using Carter;
using Checkpoint.API.Interfaces;
using Checkpoint.API.ResponseHandler;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;

namespace Checkpoint.API.Features.Project.Query
{
    internal static class GetProjectById
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
                        return ResponseDto<Dto.Response>.Fail("Proje bulunamadı!!!", 400);

                    var response = new Dto.Response()
                    {
                        Id = request.RequestDto.Id,
                        ProjectName = project.ProjectName,
                    };
                    return ResponseDto<Dto.Response>.Success(response, 200);
                }
            }
        }
        internal sealed class Dto
        {
            internal sealed class Response
            {
                public int Id { get; set; }
                public string ProjectName { get; set; }

            }

            internal sealed record Request(int Id);
        }
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
