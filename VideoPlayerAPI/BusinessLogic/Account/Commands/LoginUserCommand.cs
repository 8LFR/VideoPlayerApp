using Microsoft.EntityFrameworkCore;
using VideoPlayerAPI.Abstractions;
using VideoPlayerAPI.BusinessLogic.Account.Mappers;
using VideoPlayerAPI.BusinessLogic.Users.Models;
using VideoPlayerAPI.Infrastructure.Account;
using VideoPlayerAPI.Infrastructure.CqrsWithValidation;

namespace VideoPlayerAPI.BusinessLogic.Account.Commands;

public class LoginUserCommand : ICommand<UserToken>
{
    public string Name { get; set; }
    public string Password { get; set; }
}

internal class LoginUserCommandHandler(VideoPlayerDbContext dbContext, ITokenService tokenService) : ICommandHandler<LoginUserCommand, UserToken>
{
    private readonly VideoPlayerDbContext _dbContext = dbContext;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<Result<UserToken>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Name == command.Name);

        var token = _tokenService.CreateToken(user);

        return user.ToModelWithToken(token);
    }
}