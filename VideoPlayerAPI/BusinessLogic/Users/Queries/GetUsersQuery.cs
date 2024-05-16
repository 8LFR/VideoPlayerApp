using VideoPlayerAPI.Abstractions.Repositories;
using VideoPlayerAPI.Infrastructure.CqrsWithValidation;
using VideoPlayerAPI.Infrastructure.Image.Storages;

namespace VideoPlayerAPI.BusinessLogic.Users.Queries;

public class GetUsersQuery : IQuery<IEnumerable<Models.User>>
{
}

internal class GetUsersQueryHandler(IUserRepository userRepository, IImageStorage imageStorage) : IQueryHandler<GetUsersQuery, IEnumerable<Models.User>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IImageStorage _imageStorage = imageStorage;

    public async Task<Result<IEnumerable<Models.User>>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync();

        var models = new List<Models.User>();

        foreach (var user in users)
        {
            var model = new Models.User
            {
                Id = user.Id,
                Name = user.Name,
                Created = user.Created,
                LastActive = user.LastActive,
                AvatarUrl = _imageStorage.GetUserAvatarUrl(user.AvatarFilename)
            };

            models.Add(model);
        }

        return models;
    }
}