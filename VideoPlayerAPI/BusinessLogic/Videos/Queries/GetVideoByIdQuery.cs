using VideoPlayerAPI.Abstractions.Repositories;
using VideoPlayerAPI.BusinessLogic.Account.Mappers;
using VideoPlayerAPI.BusinessLogic.Infrastructure.Extensions;
using VideoPlayerAPI.Infrastructure.CqrsWithValidation;
using VideoPlayerAPI.Infrastructure.Image.Storages;
using VideoPlayerAPI.Infrastructure.Video.Storages;

namespace VideoPlayerAPI.BusinessLogic.Videos.Queries;

public class GetVideoByIdQuery : IQuery<Models.Video>
{
    public Guid Id { get; set; }
}

internal class GetVideoByIdQueryHandler(
    IVideoStorage videoStorage,
    IImageStorage imageStorage,
    IVideoRepository videoRepository) : IQueryHandler<GetVideoByIdQuery, Models.Video>
{
    private readonly IVideoStorage _videoStorage = videoStorage;
    private readonly IImageStorage _imageStorage = imageStorage;
    private readonly IVideoRepository _videoRepository = videoRepository;

    public async Task<Result<Models.Video>> Handle(GetVideoByIdQuery query, CancellationToken cancellationToken)
    {
        var video = await _videoRepository.GetVideoByIdAsync(query.Id);

        var avatarFilename = _imageStorage.GetUserAvatarUrl(video.UploadedBy.AvatarFilename);

        var model = new Models.Video
        {
            Id = video.Id,
            Title = video.Title,
            Description = video.Description,
            VideoUrl = _videoStorage.GetVideoUrl(video.VideoFilename),
            ThumbnailUrl = _imageStorage.GetImageUrl(video.ThumbnailFilename),
            UploadDate = video.UploadDate,
            Duration = video.Duration,
            UploadDateInfo = video.UploadDate.GetCreationInfo(),
            UploadedBy = video.UploadedBy.ToModelWithAvatar(avatarFilename)
        };

        return model;
    }
}
