using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoPlayerAPI.Abstractions;
using VideoPlayerAPI.Abstractions.Models;

namespace VideoPlayerAPI.Controllers;

public class BuggyController : BaseApiController
{
    private readonly VideoPlayerDbContext _dbContext;

    public BuggyController(ISender sender, VideoPlayerDbContext dbContext) : base(sender)
    {
        _dbContext = dbContext;
    }

    [Authorize]
    [HttpGet("auth")]
    public ActionResult<string> GetSecret()
    {
        return "secret text";
    }

    [HttpGet("not-found")]
    public ActionResult<User> GetNotFound()
    {
        var thing = _dbContext.Users.Find(Guid.Parse("00000000-0000-0000-0000-000000000000"));

        if (thing == null) return NotFound();

        return thing;
    }

    [HttpGet("server-error")]
    public ActionResult<string> GetServerError()
    {
        var thing = _dbContext.Users.Find(Guid.Parse("00000000-0000-0000-0000-000000000000"));

        var thingToReturn = thing.ToString();

        return thingToReturn;
    }

    [HttpGet("bad-request")]
    public ActionResult<string> GetBadRequest()
    {
        return BadRequest("This was not a good request");
    }
}
