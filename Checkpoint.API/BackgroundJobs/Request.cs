﻿using Checkpoint.API.Interfaces;
using EventStore.Client;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;

namespace Checkpoint.API.BackgroundJobs
{
    public class Request(IApplicationDbContext checkpointDbContext, HttpClient httpClient, EventStoreClient eventStoreClient)
    {
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        public async Task ExecuteJob(CancellationToken cancellationToken)
        {
            var actions = await checkpointDbContext.Action.Include(y => y.Controller)
                  .ThenInclude(y => y.BaseUrl)
                  .ThenInclude(y => y.Project).ToListAsync();

            if (actions.Any())
            {
                foreach (var _action in actions)
                {
                    await Parallel.ForEachAsync(actions, async (_action, ct) =>
                    {
                        var stopWatch = new Stopwatch();
                        stopWatch.Start();
                        List<string> paths = new List<string>()
                        {
                                _action.Controller!.BaseUrl!.BasePath,
                                _action.Controller.ControllerPath,
                                _action.ActionPath
                        };
                        using (HttpRequestMessage httpRequestMessage = new())
                        {
                            string url = string.Join("/", paths);

                            if (_action.Header != null)
                            {
                                foreach (var header in _action.Header)
                                {
                                    if (header.Value is JsonElement element)
                                    {
                                        httpRequestMessage.Headers.Add(header.Key, header.Value.ToString());
                                    }
                                }
                            }
                            if (_action.Body != null)
                            {
                                Dictionary<string, object> bodyDict = new();
                                foreach (var body in _action.Body)
                                {
                                    if (body.Value is JsonElement element)
                                    {
                                        RequestPayloadDeserializer.ParseJsonElementValue(element, out object data);
                                        bodyDict[body.Key] = data;
                                    }
                                }
                                httpRequestMessage.Content = JsonContent.Create(bodyDict);
                            }
                            string endUrl = url;
                            string queryUrl = string.Empty;
                            if (_action.Query != null)
                            {
                                List<string> queries = new List<string>();
                                foreach (var query in _action.Query)
                                {
                                    if (query.Value is JsonElement element)
                                    {
                                        queries.Add($"{query.Key}={Uri.EscapeDataString(query.Value.ToString())}");
                                    }
                                }
                                queryUrl = string.Join("&", queries);
                            }
                            if (queryUrl != string.Empty)
                            {
                                endUrl = string.Join("?", queryUrl);
                            }
                            httpRequestMessage.RequestUri = new Uri(endUrl);

                            httpRequestMessage.Method = _action.RequestType switch
                            {
                                Enums.RequestType.Get => HttpMethod.Get,
                                Enums.RequestType.Put => HttpMethod.Put,
                                Enums.RequestType.Delete => HttpMethod.Delete,
                                Enums.RequestType.Post => HttpMethod.Post
                            };
                            HttpResponseMessage httpResponseMessage = new();
                            Events.RequestEvent @event = new Events.RequestEvent();
                            try
                            {
                                httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                                stopWatch.Stop();
                                long responseTime = stopWatch.ElapsedMilliseconds;

                                @event = new Events.RequestEvent()
                                {
                                    RequestStatus = httpResponseMessage.IsSuccessStatusCode,
                                    ResponseTimeMs = responseTime,
                                    StatusCode = (int)httpResponseMessage.StatusCode,
                                    TimeStamp = DateTime.UtcNow,
                                    Url = endUrl,
                                    TeamId = _action.Controller.BaseUrl.Project.TeamId,
                                    IndividualId = _action.Controller.BaseUrl.Project.IndividualId
                                };


                            }
                            catch (Exception)
                            {
                                @event = new Events.RequestEvent()
                                {
                                    RequestStatus = false,
                                    ResponseTimeMs = 0,
                                    StatusCode = 0,
                                    TimeStamp = DateTime.UtcNow,
                                    Url = endUrl,
                                    TeamId = _action.Controller.BaseUrl.Project.TeamId,
                                    IndividualId = _action.Controller.BaseUrl.Project.IndividualId
                                };

                            }
                            EventData @eventData = new(
                                   eventId: Uuid.NewUuid(),
                                   type: @event.GetType().Name,
                                   data: JsonSerializer.SerializeToUtf8Bytes(@event));

                            if (cancellationToken.IsCancellationRequested)
                            {
                                return;
                            }
                            await semaphore.WaitAsync(cancellationToken);
                            try
                            {
                                await eventStoreClient.AppendToStreamAsync(
                                 streamName: endUrl,
                                 expectedState: StreamState.Any,
                                 eventData: [@eventData]);
                            }
                            finally
                            {
                                semaphore.Release();
                            }


                        }
                    });
                }



            }

        }
    }
}
