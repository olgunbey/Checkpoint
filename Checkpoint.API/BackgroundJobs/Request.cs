using Checkpoint.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;

namespace Checkpoint.API.BackgroundJobs
{
    public class Request(IApplicationDbContext checkpointDbContext, HttpClient httpClient)
    {
        public async Task ExecuteJob(CancellationToken cancellationToken)
        {
            var actions = await checkpointDbContext.Action.Include(y => y.Controller)
                  .ThenInclude(y => y.BaseUrl).ToListAsync();

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

                HttpRequestMessage httpRequestMessage = new();
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
                    string queryUrl = string.Join("&", queries);

                    string endUrl = string.Join("?", url, queryUrl);
                    httpRequestMessage.RequestUri = new Uri(endUrl);

                    httpRequestMessage.Method = _action.RequestType switch
                    {
                        Enums.RequestType.Get => HttpMethod.Get,
                        Enums.RequestType.Put => HttpMethod.Put,
                        Enums.RequestType.Delete => HttpMethod.Delete,
                        Enums.RequestType.Post => HttpMethod.Post,
                    };

                    HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                    stopWatch.Stop();
                    long responseTime = stopWatch.ElapsedMilliseconds;
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {

                    }
                    else
                    {

                    }

                }
            });
        }
    }
}
