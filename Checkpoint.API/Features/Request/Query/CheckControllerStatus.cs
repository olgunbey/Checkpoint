using Carter;
using Checkpoint.API.Events;
using Checkpoint.API.Interfaces;
using Checkpoint.API.ResponseHandler;
using EventStore.Client;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Common;
using System.Text.Json;

namespace Checkpoint.API.Features.Request.Query
{
    internal static class CheckControllerStatus
    {
        internal sealed class Mediatr
        {
            internal sealed class Request : CustomIRequest<List<Dto.Response>>
            {

            }
            internal sealed class Handler(EventStoreClient eventStoreClient, IApplicationDbContext applicationDbContext) : CustomIRequestHandler<Request, List<Dto.Response>>
            {
                public async Task<ResponseDto<List<Dto.Response>>> Handle(Request request, CancellationToken cancellationToken)
                {
                    var controllers = await applicationDbContext.Controller
                         .Include(y => y.Actions)
                         .Include(y => y.BaseUrl)
                         .ToListAsync();


                    List<Dto.Response> response = new List<Dto.Response>();
                    foreach (var controller in controllers)
                    {
                        List<RequestEvent> requestEvents = new List<RequestEvent>();
                        int successCount = 0;
                        int unSuccessCount = 0;
                        List<string> requestUrls = new List<string>()
                        {
                            controller.BaseUrl.BasePath,
                            controller.ControllerPath,
                        };

                        var endUrl = string.Join("/", requestUrls);

                        if (controller.Actions.Select(y => y.Query).Any() && controller.Actions.Select(y => y.Query) != null)
                        {

                            foreach (var action in controller.Actions)
                            {
                                string finishEndUrl = "";
                                finishEndUrl = string.Join("/", endUrl, action.ActionPath);
                                if (action.Query != null)
                                {
                                    string queryUrl = string.Join("&", action.Query.Where(y => y.Value != null)
                                  .Select(y => $"{y.Key}={Uri.EscapeDataString(y.Value.ToString()!)}"));
                                    endUrl = string.Join("?", endUrl, queryUrl);
                                }


                                var getAll = eventStoreClient.ReadStreamAsync(
                                direction: Direction.Forwards,
                                streamName: finishEndUrl,
                                revision: StreamPosition.Start);

                                var getAllEvents = await getAll.ToListAsync();


                                foreach (var requestEvent in getAllEvents)
                                {
                                    var type = requestEvent.Event.EventType;
                                    Type selectType = type switch
                                    {
                                        nameof(RequestEvent) => typeof(RequestEvent)
                                    };
                                    object deserializerEvent = JsonSerializer.Deserialize(requestEvent.Event.Data.ToArray(), selectType)!;

                                    switch (deserializerEvent)
                                    {
                                        case RequestEvent req:
                                            requestEvents.Add(req);
                                            break;
                                    }
                                }
                            }
                        }
                        var groupBy = requestEvents.GroupBy(u =>
                        {
                            return u.Url.Split('/')[0] + '/' + u.Url.Split('/')[1] + '/' + u.Url.Split('/')[2] + '/' + u.Url.Split('/')[3] + '/' + u.Url.Split('/')[4];
                        });

                        foreach (var groupByEvents in groupBy)
                        {
                            foreach (var _groupBy in groupByEvents)
                            {
                                if (_groupBy.RequestStatus)
                                {
                                    successCount++;
                                }
                                else
                                {
                                    unSuccessCount++;
                                }
                            }
                            response.Add(new Dto.Response
                            {
                                Controller = groupByEvents.Key,
                                SuccessCount = successCount,
                                UnSuccessCount = unSuccessCount
                            });
                        }
                    }
                    return ResponseDto<List<Dto.Response>>.Success(response, 200);
                }
            }
        }
        internal sealed class Dto
        {
            internal sealed record Response
            {
                public string Controller { get; set; }
                public int SuccessCount { get; set; }
                public int UnSuccessCount { get; set; }

            }
        }

        public sealed class Endpoint : ApiResponseController, ICarterModule
        {
            public void AddRoutes(IEndpointRouteBuilder app)
            {
                app.MapGet("/api/endpoint/CheckControllerStatus", Handle);
            }
            public async Task<IActionResult> Handle([FromServices] IMediator mediator, HttpContext httpContext)
            {
                var response = await mediator.Send(new Mediatr.Request());
                return Handlers(response, httpContext);
            }
        }
    }
}
