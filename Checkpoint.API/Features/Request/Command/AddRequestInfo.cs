﻿using Carter;
using Checkpoint.API.Interfaces;
using FluentValidation;
using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared;
using Shared.Common;

namespace Checkpoint.API.Features.Request.Command
{
    internal static class AddRequestInfo
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

                    var baseUrlFilter = applicationDbContext.BaseUrl
                        .Where(y => y.Id == request.RequestDto.BaseUrlId)
                        .Include(y => y.Controllers.Where(x => x.Id == request.RequestDto.ControllerId));


                    if (request.RequestDto.ControllerId != 0)
                    {

                        Entities.Action addAction = new()
                        {
                            ControllerId = request.RequestDto.ControllerId,
                            ActionPath = request.RequestDto.ActionPath,
                            Query = JsonConvert.DeserializeObject<List<RequestPayloads.Query>>(request.RequestDto.Query),
                            Body = JsonConvert.DeserializeObject<List<RequestPayloads.Body>>(request.RequestDto.Body),
                            Header = JsonConvert.DeserializeObject<List<RequestPayloads.Header>>(request.RequestDto.Header),
                            RequestType = request.RequestDto.RequestType,
                        };

                        var getBaseUrl = await baseUrlFilter
                            .ThenInclude(y => y.Actions)
                             .SingleAsync(y => y.Controllers.Any(x => x.Id == request.RequestDto.ControllerId));


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
                                    Query = JsonConvert.DeserializeObject<List<RequestPayloads.Query>>(request.RequestDto.Query),
                                    Header = JsonConvert.DeserializeObject<List<RequestPayloads.Header>>(request.RequestDto.Header),
                                    Body = JsonConvert.DeserializeObject<List<RequestPayloads.Body>>(request.RequestDto.Body),
                                    RequestType = request.RequestDto.RequestType
                                }
                            }

                        });
                    }
                    await applicationDbContext.SaveChangesAsync(cancellationToken);
                    return ResponseDto<NoContent>.Success(204);
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
        public sealed class Endpoint : ResultController, ICarterModule
        {
            public void AddRoutes(IEndpointRouteBuilder app)
            {
                app.MapPost("/api/request/addrequestInfo", Handler);/*.AddEndpointFilter<EndpointFilter>();*/
            }
            public async Task<IActionResult> Handler([FromBody] Dto.Request requestDto, IMediator mediator, HttpContext httpContext)
            {
                var response = await mediator.Send(new Mediatr.Request() { RequestDto = requestDto });
                return Handlers(httpContext, response);
            }
        }
        //public class EndpointFilter : IEndpointFilter
        //{
        //    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        //    {
        //        var requestDto = context.Arguments.FirstOrDefault(arg => arg is Dto.Request) as Dto.Request;
        //        if (context.HttpContext.Items.TryGetValue("AdminByPass", out object? data))
        //        {
        //            return await next(context);
        //        }


        //        var userTeams = context.HttpContext.User.Claims.FirstOrDefault(y => y.Type == "teams");
        //        if (userTeams == null)
        //        {
        //            return Results.Forbid();
        //        }

        //        var deserializerData = JsonConvert.DeserializeObject<List<CorporateJwtModel>>(userTeams.Value);
        //        if (!deserializerData.Any(y => y.Permissions.Any(y => y == Permission.Ekleme)))
        //        {
        //            return Results.Forbid();
        //        }
        //        return await next(context);
        //    }
        //}

    }
}
