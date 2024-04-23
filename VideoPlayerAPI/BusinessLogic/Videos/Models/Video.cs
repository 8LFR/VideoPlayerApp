namespace VideoPlayerAPI.BusinessLogic.Videos.Models;

public class Video
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public string VideoUrl { get; init; }
    public string ThumbnailUrl { get; init; }
    public DateTimeOffset UploadDate { get; init; }
    public TimeSpan Duration { get; set; }
}
