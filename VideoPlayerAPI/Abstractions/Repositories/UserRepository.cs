using Microsoft.EntityFrameworkCore;
using VideoPlayerAPI.Abstractions.Models;

namespace VideoPlayerAPI.Abstractions.Repositories;

public interface IUserRepository
{
    void Update(User user);
    Task<bool> SaveAllAsync();
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> GetUserByIdAsync(Guid id);
    Task<User> GetUserByNameAsync(string name);
}

public class UserRepository(VideoPlayerDbContext dbContext) : IUserRepository
{
    private readonly VideoPlayerDbContext _dbContext = dbContext;

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(Guid id)
    {
        return await _dbContext.Users.FindAsync(id);
    }

    public async Task<User> GetUserByNameAsync(string name)
    {
        return await _dbContext.Users.SingleOrDefaultAsync(u => u.Name == name);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public void Update(User user)
    {
        _dbContext.Entry(user).State = EntityState.Modified;
    }
}
