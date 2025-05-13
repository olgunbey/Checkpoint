using Carter;
using Checkpoint.API.Interfaces;
using Checkpoint.API.ResponseHandler;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;

namespace Checkpoint.API.Features.BaseUrl.Query
{
    internal static class GetBaseUrlAccordingToProjectId
    {
        internal sealed class Mediatr
        {
            internal sealed class Request : CustomIRequest<List<Dto.Response>>
            {
                public Dto.Request RequestDto { get; set; }
            }
            internal sealed class Handler(IApplicationDbContext applicationDbContext) : CustomIRequestHandler<Request, List<Dto.Response>>
            {
                public async Task<Shared.Common.ResponseDto<List<Dto.Response>>> Handle(Request request, CancellationToken cancellationToken)
                {
                    var getProject = await applicationDbContext.Project.FindAsync(request.RequestDto.ProjectId);
                    if (getProject == null)
                    {
                        throw new NotFoundProjectException("proje bulunamadı");
                    }

                    await applicationDbContext.Project.Entry(getProject).Collection(y => y.BaseUrls).LoadAsync(cancellationToken);

                    var responseData = getProject.BaseUrls.Select(y => new Dto.Response()
                    {
                        Id = y.Id,
                        BaseUrl = y.BasePath,
                    }).ToList();

                    return ResponseDto<List<Dto.Response>>.Success(responseData, 200);
                }
            }
        }
        internal sealed class NotFoundProjectException(string msg) : Exception(msg) { }
        internal sealed class Dto
        {
            internal sealed record Request
            {
                public int ProjectId { get; set; }
            }
            internal sealed record Response
            {
                public int Id { get; set; }
                public string BaseUrl { get; set; }
            }
        }

        public sealed class Endpoint : ApiResponseController, ICarterModule
        {
            public void AddRoutes(IEndpointRouteBuilder app)
            {
                app.MapGet("api/project/getBaseUrlAccordingToProjectId", Handle);
            }
            public async Task<IActionResult> Handle([FromServices] IMediator mediator, [FromQuery] int projectId, HttpContext httpContext)
            {
                var response = await mediator.Send(new Mediatr.Request()
                {
                    RequestDto = new Dto.Request() { ProjectId = projectId },
                });
                return Handlers(response, httpContext);
            }
        }
    }
}
