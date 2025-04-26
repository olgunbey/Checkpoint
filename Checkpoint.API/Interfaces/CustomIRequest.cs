using Checkpoint.API.Common;
using MediatR;
using Shared.Common;

namespace Checkpoint.API.Interfaces
{
    public interface CustomIRequest<T> : IRequest<ResponseDto<T>>
    {
    }
}
