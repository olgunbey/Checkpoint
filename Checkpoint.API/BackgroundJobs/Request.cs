using Checkpoint.API.Interfaces;
using System.Text.Json;

namespace Checkpoint.API.BackgroundJobs
{
    public class Request(IApplicationDbContext checkpointDbContext, HttpClient httpClient)
    {
        public async Task ExecuteJob(CancellationToken cancellationToken)
        {
            var action = checkpointDbContext.Action.FirstOrDefault();

            Dictionary<string, object> bodyDict = new();
            Dictionary<string, object> queryDict = new();
            Dictionary<string, object> headerDict = new();
            if (action == null)
            {
                return;
            }
            if (action.Query != null)
            {
                foreach (var item in action.Query!)
                {
                    if (item.Value is JsonElement element)
                    {
                        RequestPayloadDeserializer.ParseJsonElementValue(element, out object data);
                        queryDict[item.Key] = data;
                    }

                }
            }
            if (action.Body != null)
            {
                foreach (var item in action.Query!)
                {
                    if (item.Value is JsonElement element)
                    {
                        RequestPayloadDeserializer.ParseJsonElementValue(element, out object data);
                        bodyDict[item.Key] = data;
                    }
                }
            }
            if (action.Header != null)
            {
                foreach (var item in action.Query!)
                {
                    if (item.Value is JsonElement element)
                    {
                        RequestPayloadDeserializer.ParseJsonElementValue(element, out object data);
                        headerDict[item.Key] = data;
                    }

                }
            }

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();

            HttpMethod httpMethod = action.RequestType switch
            {
                Enums.RequestType.Get => HttpMethod.Get,
                Enums.RequestType.Put => HttpMethod.Put,
                Enums.RequestType.Delete => HttpMethod.Delete,
                Enums.RequestType.Post => HttpMethod.Post,
            };
            httpRequestMessage.Method = httpMethod;

            if (queryDict.Any())
            {
                string queryString = string.Join("&", queryDict.Where(kvp => kvp.Value != null)
                      .Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value.ToString())}"));

                string url = $"https://localhost:5000/api/user/getUser?{queryString}";

                httpRequestMessage.RequestUri = new Uri(url);

            }
            if (headerDict.Any())
            {
                foreach (var item in headerDict)
                {
                    httpRequestMessage.Headers.Add(item.Key, item.Value.ToString());
                }
            }
            if (bodyDict.Any())
            {
                httpRequestMessage.Content = JsonContent.Create(bodyDict);
            }


            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                Console.WriteLine("Başarılı");
            }
            else
            {
                Console.WriteLine("Başarısız");
            }


            return;
        }
    }
}
