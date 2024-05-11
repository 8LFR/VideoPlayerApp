using VideoPlayerAPI.Abstractions.Repositories;
using VideoPlayerAPI.Infrastructure.CqrsWithValidation;

namespace VideoPlayerAPI.BusinessLogic.Users.Queries;

public class GetUserByIdQuery : IQuery<Models.User>
{
    public Guid Id { get; set; }
}

internal class GetUserByIdQueryHandler(IUserRepository userRepository) : IQueryHandler<GetUserByIdQuery, Models.User>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<Models.User>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(query.Id);

        var model = new Models.User
        {
            Id = user.Id,
            Name = user.Name,
            Created = user.Created,
            LastActive = user.LastActive
        };

        return model;
    }
}