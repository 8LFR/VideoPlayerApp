using MediatR;
using Microsoft.EntityFrameworkCore;
using VideoPlayerAPI.Abstractions;

namespace VideoPlayerAPI.BusinessLogic.Users.Queries;

public class GetUsersQuery : IRequest<IEnumerable<Models.User>>
{
}

internal class GetUsersQueryHandler(VideoPlayerDbContext dbContext) : IRequestHandler<GetUsersQuery, IEnumerable<Models.User>>
{
    private readonly VideoPlayerDbContext _dbContext = dbContext;

    public async Task<IEnumerable<Models.User>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
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