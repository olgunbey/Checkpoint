using Checkpoint.API.Interfaces;
using Shared.Common;

namespace Checkpoint.API.Features.BaseUrl.Command
{
    internal static class AddBaseUrl
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
                    try
                    {
                        applicationDbContext.BaseUrl.Add(new Entities.BaseUrl()
                        {
                            ProjectId = request.RequestDto.ProjectId,
                            BasePath = request.RequestDto.BaseUrl,
                        });
                        await applicationDbContext.SaveChangesAsync(cancellationToken);

                        return ResponseDto<NoContent>.Success(201);
                    }
                    catch (Exception)
                    {
                        return ResponseDto<NoContent>.Fail("BaseUrl eklenemedi", 400);
                    }
                }
            }
        }
        internal sealed class Dto
        {
            internal sealed record Request
            {
                public string BaseUrl { get; set; }
                public int ProjectId { get; set; }
            }
        }
    }
}
