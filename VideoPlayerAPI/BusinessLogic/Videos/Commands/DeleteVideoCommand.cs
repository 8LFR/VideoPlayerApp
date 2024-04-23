using MediatR;
using VideoPlayerAPI.Abstractions;
using VideoPlayerAPI.Infrastructure.Image.Storages;
using VideoPlayerAPI.Infrastructure.Video.Storages;

namespace VideoPlayerAPI.BusinessLogic.Videos.Commands;

public class DeleteVideoCommand : IRequest<IResult>
{
    public Guid Id { get; set; }
}

internal class DeleteVideoCommandHandler(
    VideoPlayerDbContext dbContext, 
    IVideoStorage videoStorage, 
    IImageStorage imageStorage
    ) : IRequestHandler<DeleteVideoCommand, IResult>
{
    private readonly VideoPlayerDbContext _dbContext = dbContext;
    private readonly IVideoStorage _videoStorage = videoStorage;
    private readonly IImageStorage _imageStorage = imageStorage;

    public async Task<IResult> Handle(DeleteVideoCommand command, CancellationToken cancellationToken)
    {
        var video = await _dbContext.Videos.FindAsync(command.Id);

        if (video == null)
        {
            return Results.NotFound();
        }

        _dbContext.Videos.Remove(video);

        if (!string.IsNullOrWhiteSpace(video.VideoFilename))
        {
            await _videoStorage.DeleteAsync(video.VideoFilename);
        }

        if (!string.IsNullOrWhiteSpace(video.ThumbnailFilename))
        {
            await _imageStorage.DeleteAsync(video.ThumbnailFilename);
        }

        await _dbContext.SaveChangesAsync();

        return Results.Ok();
    }
}
