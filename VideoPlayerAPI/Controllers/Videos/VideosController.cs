using MediatR;
using Microsoft.AspNetCore.Mvc;
using VideoPlayerAPI.BusinessLogic.Videos.Commands;
using VideoPlayerAPI.BusinessLogic.Videos.Queries;
using VideoPlayerAPI.Models.Videos;

namespace VideoPlayerAPI.Controllers.Videos;

public class VideosController(IMediator mediator) : BaseApiController
{
    private readonly IMediator _mediator = mediator;

    // GET: api/videosa
    [HttpGet]
    public async Task<ActionResult> GetVideos()
    {
        var query = new GetVideosQuery();

        var result = await _mediator.Send(query);

        return Ok(result);
    }

    // GET: api/videos/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult> GetVideoById(Guid id)
    {
        var query = new GetVideoByIdQuery
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

    // POST: api/videos/upload
    [HttpPost("upload")]
    public async Task<ActionResult> UploadVideo([FromBody] UploadVideoWebModel webModel)
    {
        var command = new UploadVideoCommand
        {
            Title = webModel.Title,
            Description = webModel.Description,
            VideoData = webModel.VideoData,
            RequestedById = webModel.RequestedById
        };

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    // PUT: api/videos/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateVideo([FromRoute] Guid id, [FromBody] UpdateVideoWebModel webModel)
    {
        var command = new UpdateVideoCommand
        {
            Id = id,
            Title = webModel.Title,
            Description = webModel.Description
        };

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    // DELETE: api/videos/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteVideo(Guid id)
    {
        var command = new DeleteVideoCommand
        {
            Id = id
        };

        var result = await _mediator.Send(command);

        return Ok(result);
    }
}
