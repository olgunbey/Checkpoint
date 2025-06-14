using Carter;
using Checkpoint.API.Dtos;
using Checkpoint.API.Interfaces;
using Checkpoint.API.ResponseHandler;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;
using Shared.Dtos;
using Shared.Events;
using System.Security.Claims;
using System.Text.Json;

namespace Checkpoint.API.Features.Team.Query
{
    internal static class GetTeamAndProjectByUserId
    {
        internal sealed class Mediatr
        {
            internal sealed class Request : CustomIRequest<List<GetAllProjectAndTeamResponseDto>>
            {
                public Dto.Request RequestDto { get; set; }
            }
            internal sealed class Handler(IRequestClient<GetAllProjectByTeamIdEvent> requestClient) : CustomIRequestHandler<Request, List<GetAllProjectAndTeamResponseDto>>
            {
                public async Task<ResponseDto<List<GetAllProjectAndTeamResponseDto>>> Handle(Request request, CancellationToken cancellationToken)
                {
                    var response = await requestClient.GetResponse<Shared.Common.ResponseDto<List<GetAllProjectAndTeamResponseDto>>>(
                      new GetAllProjectByTeamIdEvent()
                      {
                          UserId = request.RequestDto.UserId,
                          TeamId = request.RequestDto.TeamsId
                      });

                    return response.Message;
                }
            }
        }
        internal sealed class Dto
        {
            internal sealed record Request(int UserId, List<int> TeamsId);
        }
        public class Endpoint(CorporateTokenInformationDto corporateTokenInformationDto) : ApiResponseController, ICarterModule
        {
            public void AddRoutes(IEndpointRouteBuilder app)
            {
                app.MapGet("api/team/getAllTeamAndProject", Handle).RequireAuthorization(configurePolicy =>
                configurePolicy.AddRequirements(new Requirement()));
            }
            public async Task<IActionResult> Handle([FromServices] IMediator mediatr, HttpContext httpContext)
            {
                var response = await mediatr.Send(new Mediatr.Request() { RequestDto = new Dto.Request(corporateTokenInformationDto.UserId, corporateTokenInformationDto.CorporateJwtModels.Select(y => y.TeamId).ToList()) });
                return Handlers(httpContext, response);
            }
        }
        public class Requirement : IAuthorizationRequirement
        {

        }
        public class Handler(CorporateTokenInformationDto corporateTokenInformationDto) : AuthorizationHandler<Requirement>
        {
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, Requirement requirement)
            {
                var getAllClaims = context.User.Claims.ToList();

                if (!getAllClaims.Any())
                {
                    context.Fail();
                }
                var teamsClaim = context.User.Claims.Single(y => y.Type == "teams");

                var deserData = JsonSerializer.Deserialize<List<CorporateJwtModel>>(teamsClaim.Value)!;
                int userId = int.Parse(getAllClaims.Single(y => y.Type == ClaimTypes.NameIdentifier)!.Value);


                corporateTokenInformationDto.UserId = userId;
                corporateTokenInformationDto.CorporateJwtModels = deserData;

                context.Succeed(requirement);

                return Task.CompletedTask;
            }
        }

    }
}
