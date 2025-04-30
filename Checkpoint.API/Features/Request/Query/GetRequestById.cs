using Carter;
using Checkpoint.API.Entities;
using Checkpoint.API.Enums;
using Checkpoint.API.Interfaces;
using Checkpoint.API.ResponseHandler;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Common;

namespace Checkpoint.API.Features.Request.Query
{
    internal static class Processor
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
                    BaseUrl? baseUrl = await applicationDbContext.BaseUrl.FindAsync(request.RequestDto.Id);
                    if (baseUrl == null)
                        throw new BaseUrlNotFoundException($"Not found BaseUrl-{request.RequestDto.Id} ");


                    await applicationDbContext.BaseUrl.Entry(baseUrl)
                        .Collection(y => y.Controllers)
                        .Query()
                        .Include(y => y.Actions)
                        .LoadAsync();

                    var response = new Dto.Response()
                    {
                        BasePath = baseUrl.BasePath,
                        Controllers = baseUrl.Controllers.Select(y => new Dto.Response.Controller()
                        {
                            ControllerPath = y.ControllerPath,
                            Id = y.Id,
                            Actions = y.Actions.Select(y => new Dto.Response.Action()
                            {
                                ActionPath = y.ActionPath,
                                Id = y.Id,
                                //Body = y.Body,
                                //Query = y.Query,
                                //Header = y.Header,
                                RequestType = y.RequestType
                            }).ToList()

                        }).ToList()
                    };

                    return ResponseDto<Dto.Response>.Success(response, 200);
                }
            }
        }

        internal sealed class Validator : AbstractValidator<Dto.Request>
        {
            public Validator()
            {
                RuleFor(y => y.Id).NotEmpty().NotNull().NotEqual(0);
            }

        }

        internal sealed class Dto
        {
            internal sealed record Request(int Id);
            internal sealed class Response
            {
                internal sealed class Controller
                {
                    public int Id { get; set; }
                    public string ControllerPath { get; set; }
                    public List<Action> Actions { get; set; }
                }
                internal sealed class Action
                {
                    public int Id { get; set; }
                    public string ActionPath { get; set; }
                    public RequestType RequestType { get; set; }
                    public string? Query { get; set; }
                    public string? Header { get; set; }
                    public string? Body { get; set; }
                }
                public int Id { get; set; }
                public string BasePath { get; set; }
                public List<Controller> Controllers { get; set; }

            }
        }

        internal sealed class BaseUrlNotFoundException(string msg) : Exception(msg) { }
        public sealed class Endpoint : ApiResponseController, ICarterModule
        {
            public void AddRoutes(IEndpointRouteBuilder app)
            {
                app.MapGet("api/request/getRequestById", Handler);
            }
            public async Task<IActionResult> Handler([FromQuery] int id, [FromServices] IMediator mediator, HttpContext httpContext)
            {
                var data = await mediator.Send(new Mediatr.Request() { RequestDto = new Dto.Request(id) });
                return Handlers(data, httpContext);
            }
        }
    }
}
