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

                    var controllers = getProject.BaseUrls.SelectMany(y => y.Controllers);

                    Dto.Response response = new();
                    response.ProjectName = getProject.ProjectName;

                    Dictionary<string, List<RequestEvent>> dictRequestEvent = new Dictionary<string, List<RequestEvent>>();
                    foreach (var controller in controllers)
                    {
                        List<string> requestUrls = new List<string>()
                            {
                                controller.BaseUrl!.BasePath,
                                controller.ControllerPath
                            };

                        var endUrl = string.Join("/", requestUrls);  // https://localhost:5000/api/Test


                        List<RequestEvent> requestEvents = new List<RequestEvent>();
                        foreach (var action in controller.Actions)
                        {
                            string finishUrl = string.Empty;
                            finishUrl = string.Join("/", endUrl, action.ActionPath); // https://localhost:5000/api/Test/TestAction
                            if (action.Query != null && action.Query.Any())
                            {
                                string queryUrl = string.Join("&", action.Query.Where(y => y.Value != null) //key1=values&key2=values2... 
                              .Select(y => $"{y.Key}={Uri.EscapeDataString(y.Value.ToString()!)}"));
                                finishUrl = string.Join("?", finishUrl, queryUrl); // https://localhost:5000/api/Test/TestAction?key1=values&key2=values2
                            }

                            var lastEventResult = eventStoreClient.ReadStreamAsync(
                                direction: Direction.Backwards,
                                streamName: finishUrl,
                                revision: StreamPosition.End,
                                maxCount: 1);


                            ResolvedEvent resolvedEvent = await lastEventResult.SingleAsync();

                            RequestEvent requestEvent = JsonSerializer.Deserialize<RequestEvent>(resolvedEvent.Event.Data.ToArray())!;
                            requestEvents.Add(requestEvent);
                        }
                        dictRequestEvent.Add(endUrl, requestEvents);
                    }

                    foreach (var item in dictRequestEvent)
                    {
                        int successRequestCount = item.Value.Where(y => y.RequestStatus).Count();
                        int unSuccessRequestCount = item.Value.Where(y => !y.RequestStatus).Count();
                        response.Controllers.Add(new Dto.Controller()
                        {
                            SuccessCount = successRequestCount,
                            UnSuccessCount = unSuccessRequestCount,
                            ControllerName = item.Key,
                            Actions = controllers.Where(y => y.ControllerPath == item.Key.Split('/')[4]).SelectMany(y => y.Actions)
                            .Select(y => new Dto.Action()
                            {
                                Id = y.Id,
                                ActionName = y.ActionPath
                            }).ToList()
                        });
                    }

                    return ResponseDto<Dto.Response>.Success(response, 200);

                    foreach (var controller in controllers)
                    {
                        List<RequestEvent> requestEvents = new List<RequestEvent>();

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
                                string finishUrl = string.Empty;
                                finishUrl = string.Join('/', endUrl, action.ActionPath);
                                if (action.Query != null)
                                {
                                    string queryUrl = string.Join("&", action.Query.Where(y => y.Value != null)
                                  .Select(y => $"{y.Key}={Uri.EscapeDataString(y.Value.ToString()!)}"));
                                    endUrl = string.Join("?", endUrl, queryUrl);
                                }

                                var lastEventResult = eventStoreClient.ReadStreamAsync(
                                direction: Direction.Backwards,
                                streamName: finishUrl,
                                revision: StreamPosition.End,
                                maxCount: 1);

                                var lastResolvedEvent = await lastEventResult.SingleAsync();

                                RequestEvent deserializerEvent = JsonSerializer.Deserialize<RequestEvent>(lastResolvedEvent.Event.Data.ToArray())!;
                                requestEvents.Add(deserializerEvent);

                            }
                        }
                        var groupBy = requestEvents.GroupBy(u =>
                        {
                            return u.Url.Split('/')[0] + '/' + u.Url.Split('/')[1] + '/' + u.Url.Split('/')[2] + '/' + u.Url.Split('/')[3] + '/' + u.Url.Split('/')[4];
                        });

                        //foreach (var groupByEvents in groupBy)
                        //{
                        //    foreach (var _groupBy in groupByEvents)
                        //    {
                        //        if (_groupBy.RequestStatus)
                        //        {
                        //            successCount++;
                        //        }
                        //        else
                        //        {
                        //            unSuccessCount++;
                        //        }
                        //    }

                        //    response.Controllers.Add(new Dto.Controller
                        //    {
                        //        ControllerName = groupByEvents.Key,
                        //        SuccessCount = successCount,
                        //        UnSuccessCount = unSuccessCount,
                        //        Actions = controller.Actions.Select(y => new Dto.Action { ActionName = y.ActionPath, Id = y.Id }).ToList()
                        //    });
                        //}
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
