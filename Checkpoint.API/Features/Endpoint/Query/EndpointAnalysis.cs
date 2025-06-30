using Carter;
using Checkpoint.API.Events;
using Checkpoint.API.Interfaces;
using EventStore.Client;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Common;
using System.Text.Json;

namespace Checkpoint.API.Features.Endpoint.Query
{
    internal static class EndpointAnalysis
    {
        internal sealed class Mediatr
        {
            internal sealed class Request : CustomIRequest<List<Dto.Response>>
            {
                public Dto.Request RequestDto { get; set; }
            }
            internal sealed class Handler(EventStoreClient eventStoreClient, IApplicationDbContext applicationDbContext) : CustomIRequestHandler<Request, List<Dto.Response>>
            {
                public async Task<ResponseDto<List<Dto.Response>>> Handle(Request request, CancellationToken cancellationToken)
                {
                    var getAction = (await applicationDbContext.Action.FindAsync(request.RequestDto.ActionId))!;

                    List<Dto.Response> response = new List<Dto.Response>();
                    await applicationDbContext.Action.Entry(getAction)
                         .Reference(y => y.Controller)
                         .Query()
                         .Include(y => y.BaseUrl)
                         .LoadAsync(cancellationToken);

                    string endUrl = string.Empty;
                    List<string> requestUrl = new List<string>()
                    {
                        getAction.Controller!.BaseUrl!.BasePath,
                        getAction.Controller.ControllerPath,
                        getAction.ActionPath,
                    };
                    endUrl = string.Join('/', requestUrl);

                    if (getAction.Query != null && getAction.Query.Any())
                    {
                        string queryUrl = string.Join("&", getAction.Query.Where(y => y.Value != null)
                                  .Select(y => $"{y.Key}={Uri.EscapeDataString(y.Value.ToString()!)}"));
                        endUrl = string.Join("?", endUrl, queryUrl);
                    }


                    var last5Request = eventStoreClient.ReadStreamAsync(
                         direction: Direction.Backwards,
                         streamName: endUrl,
                         revision: StreamPosition.End,
                         maxCount: 5);



                    await foreach (var req in last5Request.WithCancellation(cancellationToken))
                    {
                        RequestEvent @event = JsonSerializer.Deserialize<RequestEvent>(req.Event.Data.ToArray())!;
                        response.Add(new Dto.Response
                        {
                            ResponseStatusMs = @event.ResponseTimeMs,
                            RequestStatus = @event.RequestStatus
                        });
                    }


                    return ResponseDto<List<Dto.Response>>.Success(response, 200);
                }
            }
        }
        internal sealed class Dto
        {
            internal sealed record Response
            {
                public long ResponseStatusMs { get; set; }
                public bool RequestStatus { get; set; }

            }
            internal sealed record Request(int ActionId);
        }

        public sealed class Endpoint : ResultController, ICarterModule
        {
            public void AddRoutes(IEndpointRouteBuilder app)
            {
                app.MapGet("/api/endpoint/endpointanalysis", Handle);
            }
            public async Task<IActionResult> Handle([FromQuery] int actionId, [FromServices] IMediator mediator, HttpContext httpContext)
            {
                var response = await mediator.Send(new Mediatr.Request() { RequestDto = new Dto.Request(actionId) });
                return Handlers(httpContext, response);
            }
        }
    }
}
