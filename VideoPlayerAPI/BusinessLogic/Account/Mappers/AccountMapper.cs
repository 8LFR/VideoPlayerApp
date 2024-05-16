using VideoPlayerAPI.BusinessLogic.Users.Models;

namespace VideoPlayerAPI.BusinessLogic.Account.Mappers;

internal static class AccountMapper
{
    public static User ToModel(this Abstractions.Models.User user)
    {
        return new User
        {
            Id = user.Id,
            Name = user.Name
        };
    }

    public static User ToModelWithAvatar(this Abstractions.Models.User user, string avatarUrl)
    {
        return new User
        {
            Id = user.Id,
            Name = user.Name,
            AvatarUrl = avatarUrl
        };
    }

    public static UserToken ToModelWithToken(this Abstractions.Models.User user, string token)
    {
        return new UserToken
        {
            Id = user.Id,
            Name = user.Name,
            Token = token
        };
    }
}
