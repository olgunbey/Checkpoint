using Carter;
using Checkpoint.API.Interfaces;
using Checkpoint.Shared.Requirements;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Common;

namespace Checkpoint.API.Features.Project.Command
{
    internal static class AddProject
    {
        internal sealed class Mediatr
        {
            internal sealed class Request : CustomIRequest<NoContent>
            {
                public Dto.Request RequestDto { get; set; }
            }
            internal sealed class Handler(IApplicationDbContext applicationDbContext) : CustomIRequestHandler<Request, NoContent>
            {
                public async Task<ResponseDto<NoContent>> Handle(Request request, CancellationToken cancellationToken)
                {
                    applicationDbContext.Project.Add(new Entities.Project()
                    {
                        ProjectName = request.RequestDto.ProjectName,
                        IndividualId = request.RequestDto.IndividualId,
                        TeamId = request.RequestDto.TeamId
                    });
                    await applicationDbContext.SaveChangesAsync(cancellationToken);
                    return ResponseDto<NoContent>.Success(204);
                }
            }
        }
        internal sealed class Dto
        {
            internal sealed record Request
            {
                public string ProjectName { get; set; }
                public int? TeamId { get; set; }
                public int? IndividualId { get; set; }
            }
        }
        public sealed class Endpoint : ResultController, ICarterModule
        {
            public void AddRoutes(IEndpointRouteBuilder app)
            {
                app.MapPost("api/project/addProject", Handle).RequireAuthorization(cf => cf.AddRequirements(new AddRequirement()));
            }
            public async Task<IActionResult> Handle([FromBody] Dto.Request request, [FromServices] IMediator mediator, HttpContext httpContext)
            {
                var responseData = await mediator.Send(new Mediatr.Request() { RequestDto = request });
                return Handlers(httpContext, responseData);
            }
        }
    }
}
