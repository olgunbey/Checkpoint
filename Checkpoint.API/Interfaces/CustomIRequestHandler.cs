using Checkpoint.API.Common;
using MediatR;

namespace Checkpoint.API.Interfaces
{
    public interface CustomIRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, ResponseDto<TResponse>>
        where TRequest : IRequest<ResponseDto<TResponse>>
    {
    }
}
