using MediatR;
using Microsoft.AspNetCore.Mvc;
using VideoPlayerAPI.BusinessLogic.Videos.Commands;
using VideoPlayerAPI.BusinessLogic.Videos.Models;
using VideoPlayerAPI.BusinessLogic.Videos.Queries;
using VideoPlayerAPI.Infrastructure.CqrsWithValidation;
using VideoPlayerAPI.Models.Videos;

namespace VideoPlayerAPI.Controllers.Videos;

public class VideosController : BaseApiController
{
    private readonly IMediator _mediator;

    public VideosController(IMediator mediator, ISender sender)
        : base(sender)
    {
        _mediator = mediator;
    }

    // GET: api/videos
    [HttpGet]
    public async Task<IActionResult> GetVideos(CancellationToken cancellationToken)
    {
        var query = new GetVideosQuery();

        Result<IEnumerable<Video>> result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    // GET: api/videos/user/{id}
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserVideos(Guid userId, CancellationToken cancellationToken)
    {
        var query = new GetUserVideosQuery
        {
            Id = userId
        };

        Result<IEnumerable<Video>> result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    // GET: api/videos/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetVideoById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetVideoByIdQuery
        {
            Id = id
        };

        Result<Video> result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    // POST: api/videos/upload
    [HttpPost("upload")]
    [RequestSizeLimit(300_000_000_000)]
    public async Task<IActionResult> UploadVideo([FromBody] UploadVideoWebModel webModel, CancellationToken cancellationToken)
    {
        var command = new UploadVideoCommand
        {
            Title = webModel.Title,
            Description = webModel.Description,
            VideoData = webModel.VideoData,
            RequestedById = webModel.RequestedById
        };

        Result<Video> result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(
           nameof(GetVideoById),
           new { id = result.Value.Id },
           result.Value);
    }

    // PUT: api/videos/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVideo([FromRoute] Guid id, [FromBody] UpdateVideoWebModel webModel, CancellationToken cancellationToken)
    {
        var command = new UpdateVideoCommand
        {
            Id = id,
            Title = webModel.Title,
            Description = webModel.Description
        };

        Result<Video> result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(
           nameof(GetVideoById),
           new { id = result.Value.Id },
           result.Value);
    }

    // DELETE: api/videos/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVideo(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteVideoCommand
        {
            Id = id
        };

        Result<Result> result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent();
    }
}
