using Checkpoint.API.Entities;
using Checkpoint.API.Events;
using Checkpoint.API.Interfaces;
using EventStore.Client;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Events;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Checkpoint.API.BackgroundJobs
{
    public class Analysis(EventStoreClient eventStoreClient, IApplicationDbContext applicationDbContext, IBus bus)
    {
        public async Task ExecuteJob(CancellationToken cancellationToken)
        {

            var actionList = applicationDbContext.Action
                  .Include(y => y.Controller)
                  .ThenInclude(y => y.BaseUrl);


            var eventStoreResults = new ConcurrentDictionary<string, Dictionary<string, RequestEvent>>();

            await Parallel.ForEachAsync(actionList, async (action, ct) =>
             {
                 string requestUrl = BuildRequestUrl(action);
                 Dictionary<string, RequestEvent>? events = await ReadEventsFromEventStore(requestUrl, cancellationToken);

                 if (events != null && events.Any())
                 {
                     eventStoreResults.TryAdd(requestUrl, events);
                 }
             });

            foreach (var eventStoreResult in eventStoreResults)
            {
                var selectedRequestEvent = eventStoreResult.Value.Select(y => y.Value).ToList();

                int requestEventCount = selectedRequestEvent.Count;

                long sumResponseTime = eventStoreResult.Value.Sum(y => y.Value.ResponseTimeMs);

                double averageResponseTime = (double)sumResponseTime / requestEventCount;

                var notProcessedEvents = eventStoreResult.Value.ExceptBy(applicationDbContext.RequestedEndpointId.Select(y => y.EventId), y => y.Key).ToDictionary();

                foreach (var notProcessedEvent in notProcessedEvents)
                {
                    if ((double)notProcessedEvent.Value.ResponseTimeMs > averageResponseTime)
                    {
                        AnalysisNotAvgEvent analysisStartEvent = new()
                        {
                            IndividualId = notProcessedEvent.Value.IndividualId,
                            TeamId = notProcessedEvent.Value.TeamId,
                            ApiUrl = notProcessedEvent.Value.Url
                        };
                        var getSendEndpoint = await bus.GetSendEndpoint(new Uri($"queue:{QueueConfigurations.Checkpoint_Api_AnalysisNotAvgTime_Identity}"));
                        await getSendEndpoint.Send(analysisStartEvent);
                    }
                }
                var processedEventIds = notProcessedEvents.Keys.Select(y => new RequestedEndpointId() { EventId = y }).ToList();
                applicationDbContext.RequestedEndpointId.AddRange(processedEventIds);
                await applicationDbContext.SaveChangesAsync(cancellationToken);

            }
        }
        private string BuildRequestUrl(Entities.Action action)
        {
            string requestUrl = string.Empty;
            List<string> requestUrls = new List<string>()
                 {
                     action.Controller!.BaseUrl!.BasePath,
                     action.Controller.ControllerPath,
                     action.ActionPath
                 };
            requestUrl = string.Join("/", requestUrls);

            if (action.Query != null && action.Query.Any())
            {
                string queryUrl = string.Join("&", action.Query.Where(y => y.Value != null)
                       .Select(y => $"{y.Key}={Uri.EscapeDataString(y.Value.ToString()!)}"));

                requestUrl = string.Join("?", requestUrl, queryUrl);

            }

            return requestUrl;
        }

        private async Task<Dictionary<string, RequestEvent>?> ReadEventsFromEventStore(string requestUrl, CancellationToken cancellationToken)
        {
            var result = eventStoreClient.ReadStreamAsync(
                        direction: Direction.Backwards,
                        streamName: requestUrl,
                        revision: StreamPosition.End,
                        maxCount: 1);


            if (await result.ReadState == ReadState.StreamNotFound)
            {
                return null;
            }
            var lastEvent = await result.FirstAsync(cancellationToken);

            long lastEventNumber = lastEvent.Event.EventNumber.ToInt64();

            var eventStoreRead = eventStoreClient.ReadStreamAsync(Direction.Forwards, requestUrl, StreamPosition.Start, lastEventNumber + 1);

            Dictionary<string, RequestEvent> eventDictionary = new Dictionary<string, RequestEvent>();

            await foreach (var eventData in eventStoreRead.WithCancellation(cancellationToken))
            {

                var requestEvent = JsonSerializer.Deserialize<RequestEvent>(eventData.Event.Data.ToArray())!;

                eventDictionary.TryAdd(eventData.Event.EventId.ToString(), requestEvent);

            }
            return eventDictionary;

        }
    }
}
