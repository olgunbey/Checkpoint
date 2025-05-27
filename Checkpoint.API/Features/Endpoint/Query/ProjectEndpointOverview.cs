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
    internal static class ProjectEndpointOverview
    {
        internal sealed class Mediatr
        {
            internal sealed class Request : CustomIRequest<Dto.Response>
            {
                public Dto.Request RequestDto { get; set; }
            }
            internal sealed class Handler(IApplicationDbContext applicationDbContext, EventStoreClient eventStoreClient) : CustomIRequestHandler<Request, Dto.Response>
            {

                public async Task<ResponseDto<Dto.Response>> Handle(Request request, CancellationToken cancellationToken)
                {

                    var getProject = (await applicationDbContext.Project.FindAsync(request.RequestDto.ProjectId));

                    await applicationDbContext.Project
                         .Entry(getProject)
                         .Collection(y => y.BaseUrls)
                         .Query()
                         .Include(y => y.Controllers)
                         .ThenInclude(y => y.Actions)
                         .LoadAsync();

                    var actionList = getProject.BaseUrls
                         .SelectMany(y => y.Controllers)
                         .SelectMany(y => y.Actions)
                         .ToList();


                    List<RequestEvent> events = new List<RequestEvent>();
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

                        var lastResult = eventStoreClient.ReadStreamAsync(
                         direction: Direction.Backwards,
                         streamName: requestUrl,
                         revision: StreamPosition.End,
                         maxCount: 1
                         );

                        var selectedEndpointResolvedEvent = await lastResult.SingleAsync();


                        string eventType = selectedEndpointResolvedEvent.Event.EventType;
                        var byteEvent = selectedEndpointResolvedEvent.Event.Data.ToArray();

                        Type _typeof = eventType switch
                        {
                            nameof(RequestEvent) => typeof(RequestEvent)
                        };

                        var @event = JsonSerializer.Deserialize(byteEvent, _typeof);

                        switch (@event)
                        {
                            case RequestEvent req:
                                events.Add(req);
                                break;
                        }
                    }


                    int responsePasive = events.Where(y => !y.RequestStatus).Count();

                    int responseActive = events.Where(y => y.RequestStatus).Count();

                    int endpointCount = events.Count();

                    string lastCalledEndpoint = events.Last().Url;

                    return ResponseDto<Dto.Response>.Success(new Dto.Response { Active = responseActive, Pasive = responsePasive, EndpointCount = endpointCount }, 200);

                }
            }
        }
        internal sealed class Dto
        {
            internal sealed record Response
            {
                public int Active { get; set; }
                public int Pasive { get; set; }
                public int EndpointCount { get; set; }
            }

            internal sealed record Request(int ProjectId);
        }
        public class Endpoint : ApiResponseController, ICarterModule
        {
            public void AddRoutes(IEndpointRouteBuilder app)
            {
                app.MapGet("api/analysis/projectEndpointOverview", Handle);
            }
            public async Task<IActionResult> Handle([FromQuery] int projectId, [FromServices] IMediator mediatr, HttpContext httpContext)
            {
                var response = await mediatr.Send(new Mediatr.Request() { RequestDto = new Dto.Request(projectId) });

                return Handlers(response, httpContext);
            }
        }
    }
}
