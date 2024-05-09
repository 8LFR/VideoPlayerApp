using MediatR;

namespace VideoPlayerAPI.Infrastructure.CqrsWithValidation;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
