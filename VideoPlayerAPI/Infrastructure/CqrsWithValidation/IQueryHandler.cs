using MediatR;

namespace VideoPlayerAPI.Infrastructure.CqrsWithValidation;

public interface IQueryHandler<TQuery, TResponse>
: IRequestHandler<TQuery, Result<TResponse>>
where TQuery : IQuery<TResponse>
{
}
