using Carter;
using Checkpoint.API.ResponseHandler;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Events;

namespace Checkpoint.API.Features.Team.Query
{
    internal static class GetAllTeamAndProject
    {
        internal sealed class Mediatr
        {
            internal sealed class Request : IRequest
            {
                public Dto.Request RequestDto { get; set; }
            }
            internal sealed class Handler(IBus bus) : IRequestHandler<Request>
            {
                public async Task Handle(Request request, CancellationToken cancellationToken)
                {

                    var getSendEndpoint = await bus.GetSendEndpoint(new Uri($"queue:{QueueConfigurations.Checkpoint_Api_ListProject_Identity}"));

                    await getSendEndpoint.Send(new GetAllProjectByTeamIdEvent()
                    {
                        TeamId = request.RequestDto.TeamsId,
                    });
                }
            }
        }
        internal sealed class Dto
        {
            internal sealed record Request(int UserId, int[] TeamsId);
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
                var teamsId = getAllClaims.Where(y => y.Type == "teams").ToList().Select(y => int.Parse(y.Value)).ToArray();
                //buraları düzgün doldur
                int userId = int.Parse(getAllClaims.FirstOrDefault(y => y.Type == "")!.Value);
                await mediatr.Send(new Mediatr.Request() { RequestDto = new Dto.Request(userId, teamsId) });

                return Handlers(httpContext);
            }
        }
    }
}
