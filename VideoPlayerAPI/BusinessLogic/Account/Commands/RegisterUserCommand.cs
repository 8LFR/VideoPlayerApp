using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using VideoPlayerAPI.Abstractions;
using VideoPlayerAPI.Abstractions.Models;
using VideoPlayerAPI.BusinessLogic.Account.Mappers;

namespace VideoPlayerAPI.BusinessLogic.Account.Commands;

public class RegisterUserCommand : IRequest<Users.Models.User>
{
    public string Name { get; set; }
    public string Password { get; set; }
}

internal class RegisterUserCommandHandler(VideoPlayerDbContext dbContext) : IRequestHandler<RegisterUserCommand, Users.Models.User>
{
    private readonly VideoPlayerDbContext _dbContext = dbContext;

    public async Task<Users.Models.User> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        if (await UserExists(command.Name))
        {
            throw new Exception("Username is taken");
        }

        using var hmac = new HMACSHA512();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(command.Password)),
            PasswordSalt = hmac.Key
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return user.ToModel();
    }

    private async Task<bool> UserExists(string username)
    {
        return await _dbContext.Users.AnyAsync(u => u.Name == username.ToLower());
    }
}