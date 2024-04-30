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

    public static UserToken ToModelWithToken(this Abstractions.Models.User user, string token)
    {
        return new UserToken
        {
            Name = user.Name,
            Token = token
        };
    }
}
