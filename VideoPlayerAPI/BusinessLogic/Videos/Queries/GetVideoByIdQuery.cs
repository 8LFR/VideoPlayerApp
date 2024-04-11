using MediatR;
using VideoPlayerAPI.Abstractions;
using VideoPlayerAPI.Infrastructure.Image.Storages;
using VideoPlayerAPI.Infrastructure.Video.Storages;

namespace VideoPlayerAPI.BusinessLogic.Videos.Queries;

public class GetVideoByIdQuery : IRequest<Models.Video>
{
    public Guid Id { get; set; }
}

internal class GetVideoByIdQueryHandler(
    VideoPlayerDbContext dbContext, 
    IVideoStorage videoStorage, 
    IImageStorage imageStorage
    ) : IRequestHandler<GetVideoByIdQuery, Models.Video>
{
    private readonly VideoPlayerDbContext _dbContext = dbContext;
    private readonly IVideoStorage _videoStorage = videoStorage;
    private readonly IImageStorage _imageStorage = imageStorage;

    public async Task<Models.Video> Handle(GetVideoByIdQuery query, CancellationToken cancellationToken)
    {
        var video = await _dbContext.Videos.FindAsync(query.Id);

        var model = new Models.Video
        {
            Id = video.Id,
            Title = video.Title,
            Description = video.Description,
            VideoUrl = _videoStorage.GetVideoUrl(video.VideoFilename),
            ThumbnailUrl = _imageStorage.GetImageUrl(video.ThumbnailFilename),
            UploadDate = video.UploadDate,
            Duration = video.Duration
        };

        return model;
    }
}
