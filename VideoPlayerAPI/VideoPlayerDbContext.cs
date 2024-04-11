using Microsoft.EntityFrameworkCore;
using VideoPlayerAPI.Models;

namespace VideoPlayerAPI
{
    public class VideoPlayerDbContext(DbContextOptions<VideoPlayerDbContext> dbContext) : DbContext(dbContext)
    {
        public DbSet<Video> Videos { get; set; }
    }
}
