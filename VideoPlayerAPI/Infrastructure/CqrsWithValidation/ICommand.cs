using MediatR;

namespace VideoPlayerAPI.Infrastructure.CqrsWithValidation;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}