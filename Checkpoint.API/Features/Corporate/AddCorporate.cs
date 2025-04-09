using Checkpoint.API.Common;
using Checkpoint.API.Interfaces;

namespace Checkpoint.API.Features.User
{
    internal static class Processor
    {
        internal sealed class Request : CustomIRequest<bool>
        {
            public required Dto.AddCorporate AddCorporate { get; set; }
        }
        internal sealed class Handler : CustomIRequestHandler<Request, bool>
        {
            public Task<ResponseDto<bool>> Handle(Request request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
        internal sealed class Dto
        {
            internal sealed record AddCorporate(string mail);
        }
    }
}
