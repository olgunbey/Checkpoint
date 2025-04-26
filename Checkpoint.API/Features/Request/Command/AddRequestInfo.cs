using Carter;
using Checkpoint.API.Common;
using Checkpoint.API.Interfaces;
using Checkpoint.API.ResponseHandler;
using FluentValidation;
using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Common;

namespace Checkpoint.API.Features.Request.Command
{
    internal static class Proccessor
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

                    var baseUrlFilter = applicationDbContext.BaseUrl.Where(y => y.Id == request.RequestDto.BaseUrlId).Include(y => y.Controllers);

                    if (request.RequestDto.ControllerId != 0)
                    {

                        Entities.Action addAction = new()
                        {
                            CreateUserId = 1,
                            ControllerId = request.RequestDto.ControllerId,
                            ActionPath = request.RequestDto.ActionPath,
                        };

                        var getBaseUrl = await baseUrlFilter
                            .ThenInclude(y => y.Actions)
                             .Where(y => y.Controllers.Any(y => y.Id == request.RequestDto.ControllerId))
                             .SingleAsync();

                        getBaseUrl.Controllers.Single().Actions.Add(addAction);

                    }
                    else
                    {

                        (await baseUrlFilter.SingleAsync()).Controllers.Add(new Entities.Controller()
                        {
                            ControllerPath = request.RequestDto.ControllerPath,
                            BaseUrlId = request.RequestDto.BaseUrlId,
                            Actions = new List<Entities.Action>()
                        {
                            new Entities.Action()
                            {
                                ActionPath = request.RequestDto.ActionPath,
                            }
                        }

                        });
                    }
                    await applicationDbContext.SaveChangesAsync(cancellationToken);
                    return ResponseDto<NoContent>.Success(203);
                }
            }
        }

        internal sealed class Dto
        {
            internal sealed record Request
            {
                public int BaseUrlId { get; set; }
                public int ControllerId { get; set; }
                public string ControllerPath { get; set; }
                public string ActionPath { get; set; }
                public Enums.RequestType RequestType { get; set; }
                public string Query { get; set; }
                public string Header { get; set; }
                public string Body { get; set; }
            }
        }
        internal sealed class Validator : AbstractValidator<Entities.BaseUrl>
        {
            public Validator()
            {
                RuleFor(y => y.BasePath).NotEmpty().NotNull();
            }
        }
        public sealed class Endpoint : ApiResponseController, ICarterModule
        {
            public void AddRoutes(IEndpointRouteBuilder app)
            {
                app.MapPost("api/request/addrequestInfo", Handler);
            }
            public async Task<IActionResult> Handler([FromBody] Dto.Request requestDto, IMediator mediator, HttpContext httpContext)
            {
                var response = await mediator.Send(new Mediatr.Request() { RequestDto = requestDto });
                return Handlers(response, httpContext);
            }
        }


    }
}
