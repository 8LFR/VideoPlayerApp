using VideoPlayerAPI.Abstractions.Models;

namespace VideoPlayerAPI.Abstractions.Repositories;

public interface IUserRepository
{
    User GetUserByName(string name);
}

public class UserRepository(VideoPlayerDbContext dbContext) : IUserRepository
{
    private readonly VideoPlayerDbContext _dbContext = dbContext;

    public User GetUserByName(string name)
    {
        return _dbContext.Users.FirstOrDefault(u => u.Name == name);
    }
}
