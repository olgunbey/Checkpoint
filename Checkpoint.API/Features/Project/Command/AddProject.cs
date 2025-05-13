using Carter;
using Checkpoint.API.Interfaces;
using Checkpoint.API.ResponseHandler;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Common;
using Shared.Constants;
using Shared.Dtos;

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
        public sealed class Endpoint : ApiResponseController, ICarterModule
        {
            public void AddRoutes(IEndpointRouteBuilder app)
            {
                app.MapPost("api/project/addProject", Handle).AddEndpointFilter<EndpointFilter>();
            }
            public async Task<IActionResult> Handle([FromBody] Dto.Request request, [FromServices] IMediator mediator, HttpContext httpContext)
            {
                var responseData = await mediator.Send(new Mediatr.Request() { RequestDto = request });
                return Handlers(responseData, httpContext);
            }
        }
        public class EndpointFilter : IEndpointFilter
        {
            public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
            {
                if (context.HttpContext!.Items.TryGetValue("AdminByPass", out object data))
                {
                    return await next(context);
                }
                var teamClaim = context.HttpContext.User.Claims.FirstOrDefault(y => y.Type == "teams");

                if (teamClaim == null)
                {
                    return Results.Forbid();
                }
                var deserializerData = JsonConvert.DeserializeObject<CorporateJwtModel>(teamClaim.Value);

                if (!deserializerData.Permissions.Any(y => y == Permission.Ekleme))
                {
                    return Results.Forbid();
                }
                return await next(context);


            }
        }

    }
}
