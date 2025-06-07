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

namespace Checkpoint.API.Features.Endpoint.Query
{
    internal static class ListEndpointDetail
    {
        internal sealed class Mediatr
        {
            internal sealed class Request : CustomIRequest<Dto.Response>
            {
                public Dto.Request RequestDto { get; set; }
            }
            internal sealed class Handler(EventStoreClient eventStoreClient, IApplicationDbContext applicationDbContext) : CustomIRequestHandler<Request, Dto.Response>
            {
                public async Task<ResponseDto<Dto.Response>> Handle(Request request, CancellationToken cancellationToken)
                {

                    var getProject = (await applicationDbContext.Project.FindAsync(request.RequestDto.ProjectId))!;

                    await applicationDbContext.Project
                         .Entry(getProject)
                         .Collection(y => y.BaseUrls)
                         .Query()
                         .Include(y => y.Controllers)
                         .ThenInclude(y => y.Actions)
                         .LoadAsync();

                    var controllers = await applicationDbContext.Project
                        .SelectMany(y => y.BaseUrls)
                        .SelectMany(y => y.Controllers)
                        .ToListAsync();

                    Dto.Response response = new();
                    response.ProjectName = getProject.ProjectName;
                    foreach (var controller in controllers)
                    {
                        List<RequestEvent> requestEvents = new List<RequestEvent>();
                        int successCount = 0;
                        int unSuccessCount = 0;
                        List<string> requestUrls = new List<string>()
                        {
                            controller.BaseUrl!.BasePath,
                            controller.ControllerPath,
                        };

                        var endUrl = string.Join("/", requestUrls);

                        if (controller.Actions.Select(y => y.Query).Any() && controller.Actions.Select(y => y.Query) != null)
                        {
                            foreach (var action in controller.Actions)
                            {
                                string finisUrl = string.Empty;
                                finisUrl = string.Join('/', endUrl, action.ActionPath);
                                if (action.Query != null)
                                {
                                    string queryUrl = string.Join("&", action.Query.Where(y => y.Value != null)
                                  .Select(y => $"{y.Key}={Uri.EscapeDataString(y.Value.ToString()!)}"));
                                    endUrl = string.Join("?", endUrl, queryUrl);
                                }

                                var lastEventResult = eventStoreClient.ReadStreamAsync(
                                direction: Direction.Backwards,
                                streamName: finisUrl,
                                revision: StreamPosition.End,
                                maxCount: 1);

                                var lastResolvedEvent = await lastEventResult.SingleAsync();

                                var type = lastResolvedEvent.Event.EventType;
                                Type selectType = type switch
                                {
                                    nameof(RequestEvent) => typeof(RequestEvent)
                                };
                                object deserializerEvent = JsonSerializer.Deserialize(lastResolvedEvent.Event.Data.ToArray(), selectType)!;

                                switch (deserializerEvent)
                                {
                                    case RequestEvent req:
                                        requestEvents.Add(req);
                                        break;
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

                            response.Controllers.Add(new Dto.Controller
                            {
                                ControllerName = groupByEvents.Key,
                                SuccessCount = successCount,
                                UnSuccessCount = unSuccessCount,
                                Actions = controller.Actions.Select(y => new Dto.Action { ActionName = y.ActionPath, Id = y.Id }).ToList()
                            });
                        }
                    }
                    return ResponseDto<Dto.Response>.Success(response, 200);
                }
            }
        }
        internal sealed class Dto
        {

            internal sealed record Response
            {
                public string ProjectName { get; set; }
                public List<Controller> Controllers { get; set; } = new List<Controller>();
            }
            internal sealed record Controller
            {
                public string ControllerName { get; set; }
                public int SuccessCount { get; set; }
                public int UnSuccessCount { get; set; }
                public List<Action> Actions { get; set; }

            }
            internal sealed record Action
            {
                public int Id { get; set; }
                public string ActionName { get; set; }
            }
            internal sealed record Request(int ProjectId);
        }

        public sealed class Endpoint : ApiResponseController, ICarterModule
        {
            public void AddRoutes(IEndpointRouteBuilder app)
            {
                app.MapGet("/api/endpoint/ListEndpointDetail", Handle);
            }
            public async Task<IActionResult> Handle([FromQuery] int projectId, [FromServices] IMediator mediator, HttpContext httpContext)
            {
                var response = await mediator.Send(new Mediatr.Request() { RequestDto = new Dto.Request(projectId) });
                return Handlers(httpContext, response);
            }
        }
    }
}
