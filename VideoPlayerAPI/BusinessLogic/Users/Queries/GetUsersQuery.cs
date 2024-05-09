using Microsoft.EntityFrameworkCore;
using VideoPlayerAPI.Abstractions;
using VideoPlayerAPI.Infrastructure.CqrsWithValidation;

namespace VideoPlayerAPI.BusinessLogic.Users.Queries;

public class GetUsersQuery : IQuery<IEnumerable<Models.User>>
{
}

internal class GetUsersQueryHandler(VideoPlayerDbContext dbContext) : IQueryHandler<GetUsersQuery, IEnumerable<Models.User>>
{
    private readonly VideoPlayerDbContext _dbContext = dbContext;

    public async Task<Result<IEnumerable<Models.User>>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var users = await _dbContext.Users.ToListAsync();

        var models = new List<Models.User>();

        foreach (var user in users)
        {
            var model = new Models.User
            {
                Id = user.Id,
                Name = user.Name,
            };

            models.Add(model);
        }

        return models;
    }
}