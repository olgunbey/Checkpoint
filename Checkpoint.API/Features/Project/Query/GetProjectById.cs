using Carter;
using Checkpoint.API.Enums;
using Checkpoint.API.Interfaces;
using Checkpoint.API.ResponseHandler;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Common;

namespace Checkpoint.API.Features.Project.Query
{
    internal static class Processor
    {
        internal sealed class Mediatr
        {
            internal sealed class Request : CustomIRequest<Dto.Response>
            {
                public Dto.Request RequestDto { get; set; }
            }
            internal sealed class Handler(IApplicationDbContext applicationDbContext) : CustomIRequestHandler<Request, Dto.Response>
            {

                public async Task<ResponseDto<Dto.Response>> Handle(Request request, CancellationToken cancellationToken)
                {
                    Entities.Project? project = await applicationDbContext.Project.FindAsync(request.RequestDto.Id);

                    if (project == null)
                        throw new NotFoundProjectException("proje bulunamadı");

                    await applicationDbContext.Project.Entry(project).Collection(y => y.BaseUrls).Query()
                          .Include(y => y.Controllers)
                          .ThenInclude(y => y.Actions)
                          .LoadAsync(cancellationToken);

                    var data = new Dto.Response()
                    {
                        Id = request.RequestDto.Id,
                        ProjectName = project.ProjectName,
                        BaseUrls = project.BaseUrls.Select(y => new Dto.Response.BaseUrl()
                        {
                            BasePath = y.BasePath,
                            Id = y.Id,
                            Controllers = y.Controllers.Select(y => new Dto.Response.Controller()
                            {
                                Id = y.Id,
                                ControllerPath = y.ControllerPath,
                                Actions = y.Actions.Select(y => new Dto.Response.Action()
                                {
                                    Id = y.Id,
                                    ActionPath = y.ActionPath,
                                    RequestType = y.RequestType,
                                    Body = y.Body,
                                    Header = y.Header,
                                    Query = y.Query
                                }).ToList()
                            }).ToList()
                        }).ToList()
                    };
                    return ResponseDto<Dto.Response>.Success(data, 200);
                }
            }
        }
        internal sealed class Dto
        {
            internal sealed class Response
            {
                internal sealed class BaseUrl
                {
                    public int Id { get; set; }
                    public string BasePath { get; set; }
                    public List<Controller> Controllers { get; set; }
                }
                internal sealed class Controller
                {
                    public int Id { get; set; }
                    public string ControllerPath { get; set; }
                    public List<Action> Actions { get; set; }
                }
                internal sealed class Action
                {
                    public int Id { get; set; }
                    public string ActionPath { get; set; }
                    public RequestType RequestType { get; set; }
                    public List<RequestPayloads.Query>? Query { get; set; }
                    public List<RequestPayloads.Header>? Header { get; set; }
                    public List<RequestPayloads.Body>? Body { get; set; }
                }
                public int Id { get; set; }
                public string ProjectName { get; set; }
                public List<BaseUrl> BaseUrls { get; set; }
            }

            internal sealed record Request(int Id);
        }
        internal sealed class NotFoundProjectException(string msg) : Exception(msg) { }
        public class Endpoint : ApiResponseController, ICarterModule
        {
            public void AddRoutes(IEndpointRouteBuilder app)
            {
                app.MapGet("api/project/getProjectById", Handle);
            }
            public async Task<IActionResult> Handle([FromQuery] int id, [FromServices] IMediator mediatr, HttpContext httpContext)
            {
                var response = await mediatr.Send(new Mediatr.Request() { RequestDto = new Dto.Request(id) });

                return Handlers(response, httpContext);
            }
        }
    }


}
