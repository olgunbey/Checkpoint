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
    internal static class CheckActionStatus
    {
        internal sealed class Mediatr
        {
            internal sealed class Request : CustomIRequest<Dictionary<string, bool>>
            {

            }
            internal sealed class Handler(EventStoreClient eventStoreClient, IApplicationDbContext applicationDbContext) : CustomIRequestHandler<Request, Dictionary<string, bool>>
            {
                public async Task<ResponseDto<Dictionary<string, bool>>> Handle(Request request, CancellationToken cancellationToken)
                {
                    var actionList = await applicationDbContext.Action.
                          Include(y => y.Controller)
                          .ThenInclude(y => y.BaseUrl)
                          .ToListAsync();

                    Dictionary<string, bool> results = new Dictionary<string, bool>();
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


                        byte[] arrayByteData = lastEvent.Event.Data.ToArray();

                        var eventType = lastEvent.Event.EventType;

                        Type _typeof = eventType switch
                        {
                            nameof(RequestEvent) => typeof(RequestEvent)
                        };

                        var @event = JsonSerializer.Deserialize(arrayByteData, _typeof);

                        switch (@event)
                        {
                            case RequestEvent req:
                                results.Add(requestUrl, req.RequestStatus);
                                break;
                        }
                    }

                    return ResponseDto<Dictionary<string, bool>>.Success(results, 200);
                }
            }
        }

        public sealed class Endpoint : ApiResponseController, ICarterModule
        {
            public void AddRoutes(IEndpointRouteBuilder app)
            {
                app.MapGet("api/endpoint/CheckRequestStatus", Handler);
            }
            public async Task<IActionResult> Handler([FromServices] IMediator mediator, HttpContext httpContext)
            {
                var response = await mediator.Send(new Mediatr.Request());
                return Handlers(response, httpContext);
            }
        }

    }
}
