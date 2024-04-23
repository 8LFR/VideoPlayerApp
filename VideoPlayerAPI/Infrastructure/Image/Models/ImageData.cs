namespace VideoPlayerAPI.Infrastructure.Image.Models
{
    public record ImageData
    {
        public string ImageType { get; set; }
        public byte[] Bytes { get; set; }
    }
}
