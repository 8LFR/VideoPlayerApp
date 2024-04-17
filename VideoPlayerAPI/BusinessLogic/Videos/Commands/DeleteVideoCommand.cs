using MediatR;

namespace VideoPlayerAPI.BusinessLogic.Videos.Commands
{
    public class DeleteVideoCommand : IRequest<IResult>
    {
        public int VideoId { get; set; }
    }

    internal class DeleteVideoCommandHandler(VideoPlayerDbContext dbContext, IWebHostEnvironment environment) : IRequestHandler<DeleteVideoCommand, IResult>
    {
        private readonly VideoPlayerDbContext _dbContext = dbContext;
        private readonly IWebHostEnvironment _environment = environment;

        public async Task<IResult> Handle(DeleteVideoCommand command, CancellationToken cancellationToken)
        {
            var video = await _dbContext.Videos.FindAsync(command.VideoId);

            if(video == null)
            {
                return Results.NotFound();
            }

            var filePath = Path.Combine(_environment.WebRootPath, "uploads", video.FilePathOrUrl);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            _dbContext.Videos.Remove(video);

            await _dbContext.SaveChangesAsync();

            return Results.Ok();
        }
    }
}
