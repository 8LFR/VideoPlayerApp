using MediatR;
using Microsoft.EntityFrameworkCore;
using VideoPlayerAPI.Abstractions;
using VideoPlayerAPI.Infrastructure.Image.Storages;
using VideoPlayerAPI.Infrastructure.Video.Storages;

namespace VideoPlayerAPI.BusinessLogic.Videos.Queries;

public class GetVideosQuery : IRequest<IEnumerable<Models.Video>>
{
}

internal class GetVideosQueryHandler(
    VideoPlayerDbContext dbContext, 
    IVideoStorage videoStorage, 
    IImageStorage imageStorage
    ) : IRequestHandler<GetVideosQuery, IEnumerable<Models.Video>>
{
    private readonly VideoPlayerDbContext _dbContext = dbContext;
    private readonly IVideoStorage _videoStorage = videoStorage;
    private readonly IImageStorage _imageStorage = imageStorage;

    public async Task<IEnumerable<Models.Video>> Handle(GetVideosQuery query, CancellationToken cancellationToken)
    {
        var videos = await _dbContext.Videos.ToListAsync();

        var models = new List<Models.Video>();

        foreach (var video in videos)
        {
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

            models.Add(model);
        }

        return models;
    }
}
