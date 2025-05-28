using Carter;
using Checkpoint.API.Interfaces;
using Checkpoint.API.ResponseHandler;
using EventStore.Client;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Common;

namespace Checkpoint.API.Features.Endpoint.Query
{
    internal static class ListEndpointDetail
    {
        internal sealed class Mediatr
        {
            internal sealed class Request : CustomIRequest<List<Dto.Response>>
            {
                public Dto.Request RequestDto { get; set; }
            }
            internal sealed class Handler(IApplicationDbContext applicationDbContext, EventStoreClient eventStoreClient) : CustomIRequestHandler<Request, List<Dto.Response>>
            {

                public async Task<ResponseDto<List<Dto.Response>>> Handle(Request request, CancellationToken cancellationToken)
                {

                    var getProject = (await applicationDbContext.Project.FindAsync(request.RequestDto.ProjectId))!;


                    await applicationDbContext.Project
                         .Entry(getProject)
                         .Collection(y => y.BaseUrls)
                         .Query()
                         .Include(y => y.Controllers)
                         .ThenInclude(y => y.Actions)
                         .LoadAsync();


                    var controllerList = getProject.BaseUrls
                         .SelectMany(y => y.Controllers)
                         .ToList();

                    List<Dto.Response> responseList = new List<Dto.Response>();


                    foreach (var controller in controllerList)
                    {

                        List<string> endAction = new List<string>();
                        foreach (var action in controller.Actions)
                        {
                            string requestUrl = action.ActionPath;

                            if (action.Query != null && action.Query.Any())
                            {
                                string queryUrl = string.Join("&", action.Query.Where(y => y.Value != null)
                                       .Select(y => $"{y.Key}={Uri.EscapeDataString(y.Value.ToString()!)}"));

                                requestUrl = string.Join("?", requestUrl, queryUrl);
                            }
                            endAction.Add(requestUrl);
                        }

                        responseList.Add(new Dto.Response
                        {
                            ControllerName = controller.ControllerPath,
                            Actions = endAction.Select(y => new Dto.Action
                            {
                                Name = y
                            }).ToList()
                        });


                    }
                    return ResponseDto<List<Dto.Response>>.Success(responseList, 200);

                }
            }
        }
        internal sealed class Dto
        {
            internal sealed record Response
            {
                public string ControllerName { get; set; }
                public List<Dto.Action> Actions { get; set; }
                public int Active { get; set; }
                public int Pasive { get; set; }
            }
            internal sealed record Action
            {
                public string Name { get; set; }
            }
            internal sealed record Request(int ProjectId);
        }
        public class Endpoint : ApiResponseController, ICarterModule
        {
            public void AddRoutes(IEndpointRouteBuilder app)
            {
                app.MapGet("api/analysis/EndpointDetail", Handle);
            }
            public async Task<IActionResult> Handle([FromQuery] int projectId, [FromServices] IMediator mediatr, HttpContext httpContext)
            {
                var response = await mediatr.Send(new Mediatr.Request() { RequestDto = new Dto.Request(projectId) });

                return Handlers(response, httpContext);
            }
        }
    }
}
