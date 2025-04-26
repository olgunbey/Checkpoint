using Shared.Common;

namespace Checkpoint.API.Features.Request.Query
{
    internal interface IHandler
    {
        Task<ResponseDto<Processor.Dto.Response>> Handle(Processor.Mediatr.Request request, CancellationToken cancellationToken);
    }
}