namespace VideoPlayerAPI.Abstractions.Models;

public class Video
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string VideoFilename { get; set; }
    public string ThumbnailFilename { get; set; }
    public string ContentType { get; set; }
    public DateTimeOffset UploadDate { get; set; }
    public TimeSpan Duration { get; set; }
}
