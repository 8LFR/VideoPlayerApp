using VideoPlayerAPI.Abstractions;
using VideoPlayerAPI.Infrastructure.CqrsWithValidation;

namespace VideoPlayerAPI.BusinessLogic.Users.Queries;

public class GetUserByIdQuery : IQuery<Models.User>
{
    public Guid Id { get; set; }
}

internal class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, Models.User>
{
    private readonly VideoPlayerDbContext _dbContext;

    public GetUserByIdQueryHandler(VideoPlayerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Models.User>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync(query.Id);

        var model = new Models.User
        {
            Id = user.Id,
            Name = user.Name
        };

        return model;
    }
}