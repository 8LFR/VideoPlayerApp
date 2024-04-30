using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using VideoPlayerAPI.Abstractions;
using VideoPlayerAPI.BusinessLogic.Account.Mappers;
using VideoPlayerAPI.BusinessLogic.Users.Models;
using VideoPlayerAPI.Infrastructure.Account;

namespace VideoPlayerAPI.BusinessLogic.Account.Commands;

public class LoginUserCommand : IRequest<UserToken>
{
    public string Name { get; set; }
    public string Password { get; set; }
}

internal class LoginUserCommandHandler(VideoPlayerDbContext dbContext, ITokenService tokenService) : IRequestHandler<LoginUserCommand, UserToken>
{
    private readonly VideoPlayerDbContext _dbContext = dbContext;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<UserToken> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Name == command.Name);

        if (user == null)
        {
            throw new UnauthorizedAccessException();
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(command.Password));

        for (var i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
            {
                throw new UnauthorizedAccessException("Invalid password.");
            }
        }

        var token = _tokenService.CreateToken(user);

        return user.ToModelWithToken(token);
    }
}