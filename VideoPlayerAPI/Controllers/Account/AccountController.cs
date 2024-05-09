using MediatR;
using Microsoft.AspNetCore.Mvc;
using VideoPlayerAPI.BusinessLogic.Account.Commands;
using VideoPlayerAPI.BusinessLogic.Users.Models;
using VideoPlayerAPI.Infrastructure.CqrsWithValidation;
using VideoPlayerAPI.Models.Account;

namespace VideoPlayerAPI.Controllers.Account;

public class AccountController : BaseApiController
{
    public AccountController(ISender sender)
         : base(sender)
    {
    }

    // POST: api/account/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserWebModel webModel, CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand
        {
            Name = webModel.Name,
            Password = webModel.Password
        };

        Result<User> result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(
            nameof(Users.UsersController.GetUserById),
            new { id = result.Value.Id },
            result.Value);
    }

    // POST: api/account/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserWebModel webModel, CancellationToken cancellationToken)
    {
        var command = new LoginUserCommand
        {
            Name = webModel.Name,
            Password = webModel.Password
        };

        Result<UserToken> result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }
}
