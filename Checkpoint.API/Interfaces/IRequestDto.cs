using Checkpoint.API.Common;
using MediatR;

namespace Checkpoint.API.Interfaces
{
    public interface IRequestDto<T> : IRequest<ResponseDto<T>>
    {
    }
}
