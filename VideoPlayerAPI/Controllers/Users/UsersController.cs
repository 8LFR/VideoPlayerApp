using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoPlayerAPI.BusinessLogic.Users.Queries;

namespace VideoPlayerAPI.Controllers.Users;

[Authorize]
public class UsersController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    // GET: api/users
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult> GetUsers()
    {
        var query = new GetUsersQuery();

        var result = await _mediator.Send(query);

        return Ok(result);
    }

    // GET: api/users/{id}
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetUserById(Guid id)
    {
        var query = new GetUserByIdQuery
        {
            Id = id
        };

        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}
