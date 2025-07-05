using Carter;
using Checkpoint.API.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared;
using Shared.Common;
using Shared.Dtos;
using System.Text;

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
                app.MapPost("/api/project/addProject", Handle).RequireAuthorization(cf => cf.AddRequirements(new AuthorizationTransaction.Requirement()));
            }
            public async Task<IActionResult> Handle([FromBody] Dto.Request request, [FromServices] IMediator mediator, HttpContext httpContext)
            {
                var responseData = await mediator.Send(new Mediatr.Request() { RequestDto = request });
                return Handlers(httpContext, responseData);
            }
        }
        internal sealed class AuthorizationTransaction
        {
            internal class Requirement : IAuthorizationRequirement
            {

            }
            internal class Handler(IHttpContextAccessor httpContext) : AuthorizationHandler<Requirement>
            {
                protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, Requirement requirement)
                {
                    httpContext.HttpContext!.Request.EnableBuffering();
                    httpContext.HttpContext!.Items.TryGetValue("AdminByPass", out var adminCheck);

                    if (adminCheck is true)
                    {
                        context.Succeed(requirement);
                        return;
                    }

                    string stringBuffer;
                    using (var reader = new StreamReader(httpContext.HttpContext.Request.Body, Encoding.UTF8, leaveOpen: true))
                    {
                        stringBuffer = await reader.ReadToEndAsync();
                        httpContext.HttpContext.Request.Body.Position = 0;
                    }

                    var requestDto = JsonConvert.DeserializeObject<Dto.Request>(stringBuffer);

                    var parsedTeam = TokenTeamParsed.GetJwtTeamModel(context.User);

                    CorporateJwtTeamModel userGetSelectedTeamId = parsedTeam.Single(y => y.TeamId == requestDto.TeamId);

                    if (userGetSelectedTeamId.Permissions.Any(permission => permission == Shared.Constants.Permission.Ekleme))
                    {
                        context.Succeed(requirement);
                        return;
                    }
                    context.Fail();
                    return;

                }
            }
        }

    }
}
