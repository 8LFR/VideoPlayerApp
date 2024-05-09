using System.Security.Cryptography;
using System.Text;
using VideoPlayerAPI.Abstractions;
using VideoPlayerAPI.Abstractions.Models;
using VideoPlayerAPI.BusinessLogic.Account.Mappers;
using VideoPlayerAPI.Infrastructure.CqrsWithValidation;

namespace VideoPlayerAPI.BusinessLogic.Account.Commands;

public class RegisterUserCommand : ICommand<Users.Models.User>
{
    public string Name { get; set; }
    public string Password { get; set; }
}

internal class RegisterUserCommandHandler(VideoPlayerDbContext dbContext) : ICommandHandler<RegisterUserCommand, Users.Models.User>
{
    private readonly VideoPlayerDbContext _dbContext = dbContext;

    public async Task<Result<Users.Models.User>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
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
}