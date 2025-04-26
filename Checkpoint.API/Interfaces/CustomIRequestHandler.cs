using Checkpoint.API.Common;
using MediatR;
using Shared.Common;

namespace Checkpoint.API.Interfaces
{
    public interface CustomIRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, ResponseDto<TResponse>>
        where TRequest : IRequest<ResponseDto<TResponse>>
    {
    }
}
