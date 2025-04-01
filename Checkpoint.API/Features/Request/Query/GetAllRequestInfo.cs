using Carter;
using Checkpoint.API.Common;
using Checkpoint.API.Enums;
using Checkpoint.API.Interfaces;
using Checkpoint.API.ResponseHandler;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Checkpoint.API.Features.Request.Query
{
    internal static class Proccessor
    {
        internal sealed class Request : IRequestDto<List<ResponseDto.RequestInfoDto>>
        {

        }
        internal sealed class Handler : IRequestHandler<Request, ResponseDto<List<ResponseDto.RequestInfoDto>>>
        {
            private readonly IApplicationDbContext _applicationDbContext;
            public Handler(IApplicationDbContext applicationDbContext)
            {
                _applicationDbContext = applicationDbContext;
            }
            public async Task<ResponseDto<List<ResponseDto.RequestInfoDto>>> Handle(Request request, CancellationToken cancellationToken)
            {
                var data = _applicationDbContext.RequestInfo
                    .Include(y => y.BaseUrl)
                    .ThenInclude(y => y.Controllers)
                    .ThenInclude(y => y.Actions)
                    .Select(a => new ResponseDto.RequestInfoDto()
                    {
                        BaseUrl = new()
                        {
                            BasePath = a.BaseUrl.BasePath,
                            ControllersDto = a.BaseUrl.Controllers.Any() ? a.BaseUrl.Controllers.Select(x => new ResponseDto.ControllersDto()
                            {
                                ControllerPath = x.ControllerPath,
                                ActionsDto = x.Actions.Any() ? x.Actions.Select(z => new ResponseDto.ActionsDto()
                                {
                                    ActionPath = z.ActionPath,
                                    CreatedDate = z.CreatedDate,
                                    CreateUserId = z.CreateUserId,
                                    UpdatedDate = z.UpdatedDate,
                                    UpdateUserId = z.UpdateUserId,
                                    RequestType = z.RequestType
                                }).ToList() : Enumerable.Empty<ResponseDto.ActionsDto>().ToList(),
                                CreatedDate = x.CreatedDate,
                                UpdatedDate = x.UpdatedDate,
                                CreateUserId = x.CreateUserId,
                                UpdateUserId = x.UpdateUserId
                            }).ToList() : Enumerable.Empty<ResponseDto.ControllersDto>().ToList(),
                            UpdatedDate = a.BaseUrl.UpdatedDate,
                            CreatedDate = a.BaseUrl.CreatedDate,
                            CreateUserId = a.BaseUrl.CreateUserId,
                            UpdateUserId = a.BaseUrl.UpdateUserId
                        },
                        CreatedDate = a.CreatedDate,
                        UpdatedDate = a.UpdatedDate,
                        CreateUserId = a.CreateUserId,
                        UpdateUserId = a.UpdateUserId,
                        Body = a.Body,
                        Header = a.Header,
                        Query = a.Query

                    });
                if (await data.AnyAsync())
                    return ResponseDto<List<ResponseDto.RequestInfoDto>>.Success(await data.ToListAsync(), 200);

                return ResponseDto<List<ResponseDto.RequestInfoDto>>.Fail("Endpoint bulunamadı", 401);


            }
        }
    }
    internal sealed class ResponseDto
    {
        internal class BaseDto
        {
            public DateTime CreatedDate { get; set; }
            public int CreateUserId { get; set; }
            public DateTime UpdatedDate { get; set; }
            public int UpdateUserId { get; set; }
        }
        internal sealed class RequestInfoDto : BaseDto
        {
            public BaseUrlDto BaseUrl { get; set; }
            public string? Query { get; set; }
            public string? Header { get; set; }
            public string? Body { get; set; }
        }

        internal sealed class BaseUrlDto : BaseDto
        {
            public string BasePath { get; set; }
            public List<ControllersDto> ControllersDto { get; set; }
        }
        internal sealed class ControllersDto : BaseDto
        {
            public required string ControllerPath { get; set; }
            public List<ActionsDto> ActionsDto { get; set; }
        }
        internal sealed class ActionsDto : BaseDto
        {
            public RequestType RequestType { get; set; }
            public required string ActionPath { get; set; }
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
            app.MapGet("api/request/GetAllRequestDto", Handler);
        }
        public async Task<IActionResult> Handler(IMediator mediator)
        {
            var response = await mediator.Send(new Proccessor.Request());
            return Handlers(response);
        }
    }
}
