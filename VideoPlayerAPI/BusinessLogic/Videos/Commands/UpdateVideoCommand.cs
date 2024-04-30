using MediatR;
using VideoPlayerAPI.Abstractions;
using VideoPlayerAPI.Abstractions.Models;

namespace VideoPlayerAPI.BusinessLogic.Videos.Commands;

public class UpdateVideoCommand : IRequest<Video>
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Guid RequestedById { get; set; }
}

internal class UpdateVideoCommandHandler(VideoPlayerDbContext dbContext) : IRequestHandler<UpdateVideoCommand, Video>
{
    private readonly VideoPlayerDbContext _dbContext = dbContext;

    public async Task<Video> Handle(UpdateVideoCommand command, CancellationToken cancellationToken)
    {
        var video = await _dbContext.Videos.FindAsync(command.Id) ?? throw new NullReferenceException();

        video.Title = command.Title ?? video.Title;
        video.Description = command.Description ?? video.Description;

        await _dbContext.SaveChangesAsync();

        return video;
    }
}
