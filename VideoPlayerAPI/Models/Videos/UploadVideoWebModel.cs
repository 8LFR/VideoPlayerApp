using VideoPlayerAPI.Infrastructure.Video;

namespace VideoPlayerAPI.Models.Videos;

public class UploadVideoWebModel
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required VideoData VideoData { get; init; }
    public required Guid RequestedById { get; init; }
}
