using Carter;
using Checkpoint.API.Interfaces;
using Checkpoint.API.ResponseHandler;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;

namespace Checkpoint.API.Features.Project.Command
{
    internal static class Processor
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
                        CreateUserId = request.RequestDto.CreateUserId,
                        IndividualId = request.RequestDto.IndividualId,
                        TeamId = request.RequestDto.TeamId
                    });
                    await applicationDbContext.SaveChangesAsync(cancellationToken);
                    return ResponseDto<NoContent>.Success(204);
                }
            }
        }

        internal sealed class Validator : AbstractValidator<Dto.Request>
        {
            public Validator()
            {
                RuleFor(x => x.ProjectName).NotEmpty().NotNull();
                RuleFor(x => x.TeamId).Empty().Null();
                RuleFor(y => y.IndividualId).Empty().Null();
            }
        }
        internal sealed class Dto
        {
            internal sealed record Request
            {
                public string ProjectName { get; set; }
                public int CreateUserId { get; set; }
                public int? TeamId { get; set; }
                public int? IndividualId { get; set; }
            }
        }
        public sealed class Endpoint : ApiResponseController, ICarterModule
        {
            public void AddRoutes(IEndpointRouteBuilder app)
            {
                app.MapPost("api/project/addProject", Handle);
            }
            public async Task<IActionResult> Handle([FromBody] Dto.Request request, [FromServices] IMediator mediator, HttpContext httpContext)
            {
                var responseData = await mediator.Send(new Mediatr.Request() { RequestDto = request });
                return Handlers(responseData, httpContext);
            }
        }

    }
}
