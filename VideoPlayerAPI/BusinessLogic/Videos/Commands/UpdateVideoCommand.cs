using VideoPlayerAPI.Abstractions;
using VideoPlayerAPI.BusinessLogic.Videos.Mappers;
using VideoPlayerAPI.Infrastructure.CqrsWithValidation;

namespace VideoPlayerAPI.BusinessLogic.Videos.Commands;

public class UpdateVideoCommand : ICommand<Models.Video>
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Guid RequestedById { get; set; }
}

internal class UpdateVideoCommandHandler(VideoPlayerDbContext dbContext) : ICommandHandler<UpdateVideoCommand, Models.Video>
{
    private readonly VideoPlayerDbContext _dbContext = dbContext;

    public async Task<Result<Models.Video>> Handle(UpdateVideoCommand command, CancellationToken cancellationToken)
    {
        var video = await _dbContext.Videos.FindAsync(command.Id);

        video.Title = command.Title ?? video.Title;
        video.Description = command.Description ?? video.Description;

        await _dbContext.SaveChangesAsync();

        return video.ToModel();
    }
}
