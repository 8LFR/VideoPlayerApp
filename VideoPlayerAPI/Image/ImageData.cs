namespace VideoPlayerAPI.Image
{
    public record ImageData
    {
        public string ImageType { get; set; }
        public byte[] Bytes { get; set; }
    }
}
