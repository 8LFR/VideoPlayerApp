using MediatR;
using VideoPlayerAPI.BusinessLogic.Videos.Extensions;
using VideoPlayerAPI.Models;

namespace VideoPlayerAPI.BusinessLogic.Videos.Queries
{
    public class UploadVideoCommand : IRequest<Video>
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required IFormFile File { get; set; }
    }

    internal class UploadVideoCommandHandler(VideoPlayerDbContext dbContext, IWebHostEnvironment environment) : IRequestHandler<UploadVideoCommand, Video>
    {
        private readonly VideoPlayerDbContext _dbContext = dbContext;
        private readonly IWebHostEnvironment _environment = environment;

        public async Task<Video> Handle(UploadVideoCommand command, CancellationToken cancellationToken)
        {
            if (command.File == null || command.File.Length == 0)
            {
                throw new Exception("No file uploaded.");
            }

            var filePath = Guid.NewGuid().ToString() + Path.GetExtension(command.File.FileName);
            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads", filePath);

            using (var stream = new FileStream(uploadPath, FileMode.Create))
            {
                await command.File.CopyToAsync(stream);
            }

            var video = new Video
            {
                Title = command.Title,
                Description = command.Description,
                FilePathOrUrl = filePath,
                FileName = command.File.FileName,
                FileSize = (int)Math.Min(command.File.Length, int.MaxValue),
                ContentType = command.File.ContentType,
                UploadDate = DateTime.Now,
                Duration = VideoExtensions.GetVideoDuration(uploadPath)
            };

            _dbContext.Videos.Add(video);
            await _dbContext.SaveChangesAsync();

            return video;
        }
    }
}
