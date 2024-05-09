using VideoPlayerAPI.Abstractions;
using VideoPlayerAPI.Abstractions.Repositories;
using VideoPlayerAPI.Infrastructure.CqrsWithValidation;
using VideoPlayerAPI.Infrastructure.Image.Storages;
using VideoPlayerAPI.Infrastructure.Video.Storages;

namespace VideoPlayerAPI.BusinessLogic.Videos.Commands;

public class DeleteVideoCommand : ICommand<Result>
{
    public Guid Id { get; set; }
}

internal class DeleteVideoCommandHandler(
    VideoPlayerDbContext dbContext, 
    IVideoStorage videoStorage, 
    IImageStorage imageStorage,
    IVideoRepository videoRepository
    ) : ICommandHandler<DeleteVideoCommand, Result>
{
    private readonly VideoPlayerDbContext _dbContext = dbContext;
    private readonly IVideoStorage _videoStorage = videoStorage;
    private readonly IImageStorage _imageStorage = imageStorage;
    private readonly IVideoRepository _videoRepository = videoRepository;

    public async Task<Result<Result>> Handle(DeleteVideoCommand command, CancellationToken cancellationToken)
    {
        var video = await _dbContext.Videos.FindAsync(command.Id);

        _videoRepository.DeleteVideo(video);

        if (!string.IsNullOrWhiteSpace(video.VideoFilename))
        {
            await _videoStorage.DeleteAsync(video.VideoFilename);
        }

        if (!string.IsNullOrWhiteSpace(video.ThumbnailFilename))
        {
            await _imageStorage.DeleteAsync(video.ThumbnailFilename);
        }

        await _dbContext.SaveChangesAsync();

        return Result.Success();
    }
}
