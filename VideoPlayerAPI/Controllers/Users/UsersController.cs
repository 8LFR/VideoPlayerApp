using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoPlayerAPI.BusinessLogic.Users.Models;
using VideoPlayerAPI.BusinessLogic.Users.Queries;
using VideoPlayerAPI.Infrastructure.CqrsWithValidation;

namespace VideoPlayerAPI.Controllers.Users;

[Authorize]
public class UsersController : BaseApiController
{
    public UsersController(ISender sender)
        : base(sender)
    {
    }

    // GET: api/users
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery();

        Result<IEnumerable<User>> result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    // GET: api/users/{id}
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery
        {
            Id = id
        };

        Result<User> result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }
}
