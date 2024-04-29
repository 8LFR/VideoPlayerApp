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
                Duration = new TimeSpan(video.Duration.Hours, video.Duration.Minutes, video.Duration.Seconds),
                UploadDateInfo = GetCreationInfo(video.UploadDate)
        };

            models.Add(model);
        }

        return models;
    }

    private string GetCreationInfo(DateTimeOffset uploadDate)
    {
        var timeSinceCreation = DateTimeOffset.UtcNow - uploadDate;

        if (timeSinceCreation.TotalSeconds < 60)
        {
            return "Just now";
        }
        else if (timeSinceCreation.TotalMinutes < 60)
        {
            var minutes = (int)timeSinceCreation.TotalMinutes;
            return $"{minutes} minute{(minutes == 1 ? "" : "s")} ago";
        }
        else if (timeSinceCreation.TotalHours < 24)
        {
            var hours = (int)timeSinceCreation.TotalHours;
            return $"{hours} hour{(hours == 1 ? "" : "s")} ago";
        }
        else if (timeSinceCreation.TotalDays < 365)
        {
            var days = (int)timeSinceCreation.TotalDays;
            return $"{days} day{(days == 1 ? "" : "s")} ago";
        }
        else
        {
            var years = (int)(timeSinceCreation.TotalDays / 365);
            return $"{years} year{(years == 1 ? "" : "s")} ago";
        }
    }
}
