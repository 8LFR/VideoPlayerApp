using MediatR;
using Microsoft.AspNetCore.Mvc;
using VideoPlayerAPI.BusinessLogic.Account.Commands;
using VideoPlayerAPI.Models.Account;

namespace VideoPlayerAPI.Controllers.Account;

public class AccountController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    // POST: api/account/register
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterUserWebModel webModel)
    {
        var command = new RegisterUserCommand
        {
            Name = webModel.Name,
            Password = webModel.Password
        };

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    // POST: api/account/login
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginUserWebModel webModel)
    {
        var command = new LoginUserCommand
        {
            Name = webModel.Name,
            Password = webModel.Password
        };

        var result = await _mediator.Send(command);

        return Ok(result);
    }
}
