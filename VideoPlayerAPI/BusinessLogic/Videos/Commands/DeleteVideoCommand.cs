using MediatR;

namespace VideoPlayerAPI.BusinessLogic.Videos.Commands
{
    public class DeleteVideoCommand : IRequest<IResult>
    {
        public int VideoId { get; set; }
    }

    internal class DeleteVideoCommandHandler(VideoPlayerDbContext dbContext) : IRequestHandler<DeleteVideoCommand, IResult>
    {
        private readonly VideoPlayerDbContext _dbContext = dbContext;

        public async Task<IResult> Handle(DeleteVideoCommand command, CancellationToken cancellationToken)
        {
            var video = await _dbContext.Videos.FindAsync(command.VideoId);

            if(video == null)
            {
                return Results.NotFound();
            }

            _dbContext.Videos.Remove(video);

            await _dbContext.SaveChangesAsync();

            return Results.Ok();
        }
    }
}
