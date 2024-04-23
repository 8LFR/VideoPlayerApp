namespace VideoPlayerAPI.Infrastructure.Video;

public record VideoData
{
    public string VideoType { get; set; }
    public byte[] Bytes { get; set; }
}
