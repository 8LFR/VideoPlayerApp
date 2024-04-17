namespace VideoPlayerAPI.Models
{
    public class UploadVideoWebModel
    {
        public required string Title { get; init; }
        public required string Description { get; init; }
        public required IFormFile File { get; init; }
    }
}
