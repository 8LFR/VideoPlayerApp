using Microsoft.EntityFrameworkCore;
using VideoPlayerAPI.Abstractions.Models;

namespace VideoPlayerAPI.Abstractions.Repositories;


public interface IVideoRepository
{
    void DeleteVideo(Video video);
    Task<Video> GetVideoByIdAsync(Guid id);
    Task<IEnumerable<Video>> GetAllAsync();
}

public class VideoRepository(VideoPlayerDbContext dbContext) : IVideoRepository
{
    private readonly VideoPlayerDbContext _dbContext = dbContext;

    public void DeleteVideo(Video video)
    {
        _dbContext.Videos.Remove(video);
    }

    public async Task<IEnumerable<Video>> GetAllAsync()
    {
        return await _dbContext.Videos
            .Include(v => v.UploadedBy)
            .ToListAsync();
    }

    public async Task<Video> GetVideoByIdAsync(Guid id)
    {
        return await _dbContext.Videos
            .Include(v => v.UploadedBy)
            .SingleOrDefaultAsync(v => v.Id == id);
    }
}
