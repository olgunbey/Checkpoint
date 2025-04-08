using Checkpoint.API.Common;
using MediatR;

namespace Checkpoint.API.Interfaces
{
    public interface CustomIRequest<T> : IRequest<ResponseDto<T>>
    {
    }
}
