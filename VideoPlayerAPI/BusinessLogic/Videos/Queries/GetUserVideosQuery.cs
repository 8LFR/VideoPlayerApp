using VideoPlayerAPI.Abstractions.Repositories;
using VideoPlayerAPI.BusinessLogic.Account.Mappers;
using VideoPlayerAPI.BusinessLogic.Infrastructure.Extensions;
using VideoPlayerAPI.Infrastructure.CqrsWithValidation;
using VideoPlayerAPI.Infrastructure.Image.Storages;
using VideoPlayerAPI.Infrastructure.Video.Storages;

namespace VideoPlayerAPI.BusinessLogic.Videos.Queries;

public class GetUserVideosQuery : IQuery<IEnumerable<Models.Video>>
{
    public Guid Id { get; set; }
}

internal class GetUserVideosQueryHandler(
    IVideoStorage videoStorage,
    IImageStorage imageStorage,
    IVideoRepository videoRepository) : IQueryHandler<GetUserVideosQuery, IEnumerable<Models.Video>>
{
    private readonly IVideoStorage _videoStorage = videoStorage;
    private readonly IImageStorage _imageStorage = imageStorage;
    private readonly IVideoRepository _videoRepository = videoRepository;

    public async Task<Result<IEnumerable<Models.Video>>> Handle(GetUserVideosQuery query, CancellationToken cancellationToken)
    {
        var videos = await _videoRepository.GetAllAsync();

        var userVideos = videos.Where(v => v.UploadedById == query.Id);

        var models = new List<Models.Video>();

        foreach (var video in userVideos)
        {
            var avatarFilename = _imageStorage.GetUserAvatarUrl(video.UploadedBy.AvatarFilename);

            var model = new Models.Video
            {
                Id = video.Id,
                Title = video.Title,
                Description = video.Description,
                VideoUrl = _videoStorage.GetVideoUrl(video.VideoFilename),
                ThumbnailUrl = _imageStorage.GetImageUrl(video.ThumbnailFilename),
                UploadDate = video.UploadDate,
                Duration = new TimeSpan(video.Duration.Hours, video.Duration.Minutes, video.Duration.Seconds),
                UploadDateInfo = video.UploadDate.GetCreationInfo(),
                UploadedBy = video.UploadedBy.ToModelWithAvatar(avatarFilename)
            };

            models.Add(model);
        }

        return models;
    }
}
