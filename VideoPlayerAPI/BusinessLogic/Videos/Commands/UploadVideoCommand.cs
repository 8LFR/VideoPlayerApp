using VideoPlayerAPI.Abstractions;
using VideoPlayerAPI.Abstractions.Models;
using VideoPlayerAPI.BusinessLogic.Videos.Mappers;
using VideoPlayerAPI.Infrastructure.CqrsWithValidation;
using VideoPlayerAPI.Infrastructure.Image.Models;
using VideoPlayerAPI.Infrastructure.Image.Storages;
using VideoPlayerAPI.Infrastructure.Video;
using VideoPlayerAPI.Infrastructure.Video.Helpers;
using VideoPlayerAPI.Infrastructure.Video.Storages;

namespace VideoPlayerAPI.BusinessLogic.Videos.Queries;

public class UploadVideoCommand : ICommand<Models.Video>
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required VideoData VideoData { get; set; }
    public required Guid RequestedById { get; set; }
}

internal class UploadVideoCommandHandler(
    VideoPlayerDbContext dbContext, 
    IVideoStorage videoStorage,
    IVideoHelper videoHelper,
    IImageStorage imageStorage
    ) : ICommandHandler<UploadVideoCommand, Models.Video>
{
    private readonly VideoPlayerDbContext _dbContext = dbContext;
    private readonly IVideoStorage _videoStorage = videoStorage;
    private readonly IVideoHelper _videoHelper = videoHelper;
    private readonly IImageStorage _imageStorage = imageStorage;

    public async Task<Result<Models.Video>> Handle(UploadVideoCommand command, CancellationToken cancellationToken)
    {
        var video = new Video
        {
            Id = Guid.NewGuid(),
            Title = command.Title,
            Description = command.Description,
            ContentType = command.VideoData.VideoType,
            UploadDate = DateTimeOffset.UtcNow,
            UploadedById = command.RequestedById
        };

        var imageData = await _videoHelper.GenerateThumbnailAsync(video.Id, command.VideoData);

        video.VideoFilename = await SaveVideoAsync(video.Id, command.VideoData);
        video.ThumbnailFilename = await SaveImageAsync(video.Id, imageData);
        video.Duration = await _videoHelper.GetVideoDurationAsync(video.Id, command.VideoData);

        _dbContext.Videos.Add(video);
        await _dbContext.SaveChangesAsync();

        return video.ToModel();
    }

    private async Task<string> SaveVideoAsync(Guid videoId, VideoData videoData)
    {
        var newVideoFilename = $"{videoId}.{videoData.VideoType}";

        await _videoStorage.AddAsync(videoData.Bytes, newVideoFilename);

        return newVideoFilename;
    }

    private async Task<string> SaveImageAsync(Guid videoId, ImageData imageData)
    {
        var newImageFilename = $"{videoId}.{imageData.ImageType}";

        await _imageStorage.AddAsync(imageData.Bytes, newImageFilename);

        return newImageFilename;
    }
}
