using VideoPlayerAPI.Abstractions.Models;

namespace VideoPlayerAPI.Abstractions.Repositories;


public interface IVideoRepository
{
    void DeleteVideo(Video video);
    Video GetVideoById(Guid id);
}

public class VideoRepository : IVideoRepository
{
    private readonly VideoPlayerDbContext _dbContext;

    public VideoRepository(VideoPlayerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void DeleteVideo(Video video)
    {
        _dbContext.Videos.Remove(video);
    }

    public Video GetVideoById(Guid id)
    {
        return _dbContext.Videos.FirstOrDefault(v => v.Id == id);
    }
}
