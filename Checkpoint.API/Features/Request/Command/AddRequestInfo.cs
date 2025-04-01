using Carter;
using Checkpoint.API.Common;
using Checkpoint.API.ResponseHandler;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Checkpoint.API.Features.Request.Command
{
    internal static class Proccessor
    {
        internal sealed class Request : IRequest<ResponseDto<bool>>
        {
            internal record AddRequestDto
            {
                public string BasePath { get; set; }
            }

        }
        internal sealed class Handler : IRequestHandler<Request, ResponseDto<bool>>
        {
            public Task<ResponseDto<bool>> Handle(Request request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
    internal sealed class Validator : AbstractValidator<Entities.RequestInfo>
    {
        public Validator()
        {
            RuleFor(y => y.BaseUrlId).NotEmpty().NotNull();
        }
    }
    internal class Endpoint : ApiResponseController, ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/request/addrequestInfo", Handler);
        }
        public async Task<IActionResult> Handler([FromBody] Proccessor.Request.AddRequestDto addRequestDto, IMediator mediator)
        {
            var response = await mediator.Send(new Proccessor.Request());
            return Handlers(response);
        }
    }


}
