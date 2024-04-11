namespace VideoPlayerAPI.Models;

public class Video
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string FilePathOrUrl { get; set; }
    public string FileName { get; set; }
    public int FileSize { get; set; }
    public string ContentType { get; set; }
    public DateTime UploadDate { get; set; }
    public TimeSpan Duration { get; set; }
}
