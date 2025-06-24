using Checkpoint.API.Entities;
using Checkpoint.API.Events;
using Checkpoint.API.Interfaces;
using EventStore.Client;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Events;
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


            var actionList2 = await applicationDbContext.Action
                  .Include(y => y.Controller)
                  .ThenInclude(y => y.BaseUrl)
                  .ToListAsync();

            await Parallel.ForEachAsync(actionList, async (action, ct) =>
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

                 var eventStoreRead = eventStoreClient.ReadStreamAsync(Direction.Forwards, requestUrl, StreamPosition.Start, lastEventNumber);

                 Dictionary<string, RequestEvent> eventDictionary = new Dictionary<string, RequestEvent>();

                 var control = eventStoreRead.Select(y => eventDictionary.TryAdd(y.Event.EventId.ToString(),
                       JsonSerializer.Deserialize(y.Event.Data.ToArray(), typeof(RequestEvent)) as RequestEvent));

                 if (await control.AllAsync(y => y))
                 {
                     int dictionaryCount = eventDictionary.Count;
                     long sumResponseTime = eventDictionary.Sum(y => y.Value.ResponseTimeMs);
                     double averageResponseTime = (double)sumResponseTime / dictionaryCount;

                     var notProcessedEvents = eventDictionary.ExceptBy(applicationDbContext.RequestedEndpointId.Select(y => y.EventId), y => y.Key).ToDictionary();

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
                             var getSendEndpoint = await bus.GetSendEndpoint(new Uri($"{QueueConfigurations.Checkpoint_Api_AnalysisNotAvgTime_Identity}"));
                             await getSendEndpoint.Send(analysisStartEvent);
                         }
                     }
                     var processedEventIds = notProcessedEvents.Keys.Select(y => new RequestedEndpointId() { EventId = y }).ToList();

                     applicationDbContext.RequestedEndpointId.AddRange(processedEventIds);
                     await applicationDbContext.SaveChangesAsync(cancellationToken);
                 }
             });
        }
    }
}
