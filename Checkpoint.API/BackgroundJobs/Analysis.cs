using Checkpoint.API.Data;
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
    public class Analysis(EventStoreClient eventStoreClient, IServiceScopeFactory serviceScopeFactory, IApplicationDbContext applicationDbContext, IBus bus)
    {
        public async Task ExecuteJob(CancellationToken cancellationToken)
        {

            var actionList = await applicationDbContext.Action
                  .Include(y => y.Controller)
                  .ThenInclude(y => y.BaseUrl)
                  .ToListAsync();


            foreach (var action in actionList)
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

                var last = eventStoreClient.ReadStreamAsync(
                    direction: Direction.Backwards,
                    streamName: requestUrl,
                    revision: StreamPosition.End,
                    maxCount: 1);
                var lastEvent = await last.FirstOrDefaultAsync(cancellationToken);
                long lastEventNumber = lastEvent.Event.EventNumber.ToInt64();

                var first = eventStoreClient.ReadStreamAsync(
                    direction: Direction.Forwards,
                    streamName: requestUrl,
                    revision: StreamPosition.Start,
                    maxCount: 1);
                var firstEvent = await first.FirstOrDefaultAsync(cancellationToken);
                long firstEventNumber = firstEvent.Event.EventNumber.ToInt64();

                long count = 0;
                long current = 0;
                long currentEventCount = (lastEventNumber - firstEventNumber) + 1;

                ConcurrentDictionary<string, RequestEvent> eventDictionary = new ConcurrentDictionary<string, RequestEvent>();

                await eventStoreClient.SubscribeToStreamAsync(
                     streamName: requestUrl,
                     start: FromStream.Start,
                     eventAppeared: async (streamSubscription, resolvedEvent, cancellationToken) =>
                     {
                         using var scope = serviceScopeFactory.CreateScope();
                         var _applicationDbContext = scope.ServiceProvider.GetRequiredService<CheckpointDbContext>();
                         current = Interlocked.Increment(ref count);
                         string eventName = resolvedEvent.Event.EventType;
                         Type type = eventName switch
                         {
                             nameof(RequestEvent) => typeof(RequestEvent)
                         };
                         object @event = JsonSerializer.Deserialize(resolvedEvent.Event.Data.ToArray(), type)!;
                         switch (@event)
                         {
                             case RequestEvent requestEvent:
                                 eventDictionary.TryAdd(resolvedEvent.Event.EventId.ToString(), requestEvent);
                                 break;
                         }

                         if (currentEventCount == current)
                         {
                             int dictionaryCount = eventDictionary.Count;
                             long sumResponseTime = eventDictionary.Sum(y => y.Value.ResponseTimeMs);
                             double averageResponseTime = (double)sumResponseTime / dictionaryCount;


                             var notProcessedEvents = eventDictionary.ExceptBy(_applicationDbContext.RequestedEndpointId.Select(y => y.EventId), y => y.Key).ToDictionary();

                             foreach (var notProcessedEvent in notProcessedEvents)
                             {
                                 if ((double)notProcessedEvent.Value.ResponseTimeMs > averageResponseTime)
                                 {
                                     AnalysisNotAvgEvent analysisStartEvent = new()
                                     {
                                         IndividualId = notProcessedEvent.Value.IndividualId,
                                         TeamId = notProcessedEvent.Value.TeamId
                                     };
                                     var getSendEndpoint = await bus.GetSendEndpoint(new Uri($"{QueueConfigurations.Checkpoint_Api_AnalysisNotAvgTime_Identity}"));
                                     await getSendEndpoint.Send(analysisStartEvent);
                                     Console.WriteLine(notProcessedEvent.Value.Url + "" + notProcessedEvent.Value.ResponseTimeMs + "Artış var imdaaat!!");
                                 }
                             }
                             var processedEventIds = notProcessedEvents.Keys.Select(y => new RequestedEndpointId() { EventId = y }).ToList();

                             _applicationDbContext.RequestedEndpointId.AddRange(processedEventIds);
                             await _applicationDbContext.SaveChangesAsync(cancellationToken);

                         }


                     });
            }
        }
    }
}
