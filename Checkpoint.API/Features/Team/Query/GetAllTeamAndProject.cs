using Carter;
using Checkpoint.API.Interfaces;
using Checkpoint.API.ResponseHandler;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;
using Shared.Dtos;
using Shared.Events;
using System.Security.Claims;
using System.Text.Json;

namespace Checkpoint.API.Features.Team.Query
{
    internal static class GetAllTeamAndProject
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
        public class Endpoint : ApiResponseController, ICarterModule
        {
            public void AddRoutes(IEndpointRouteBuilder app)
            {
                app.MapGet("api/team/getAllTeamAndProject", Handle);
            }
            public async Task<IActionResult> Handle([FromServices] IMediator mediatr, HttpContext httpContext)
            {
                var getAllClaims = httpContext.User.Claims.ToList();


                var teams = getAllClaims.Where(y => y.Type == "teams").Single();

                var deserdata = JsonSerializer.Deserialize<List<CorporateJwtModel>>(teams.Value)!;

                int userId = int.Parse(getAllClaims.FirstOrDefault(y => y.Type == ClaimTypes.NameIdentifier)!.Value);
                var response = await mediatr.Send(new Mediatr.Request() { RequestDto = new Dto.Request(userId, deserdata.Select(y => y.TeamId).ToList()) });

                return Handlers(httpContext, response);
            }
        }

    }
}
