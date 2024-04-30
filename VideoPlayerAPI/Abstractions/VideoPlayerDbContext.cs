using Microsoft.EntityFrameworkCore;
using VideoPlayerAPI.Abstractions.Models;

namespace VideoPlayerAPI.Abstractions
{
    public class VideoPlayerDbContext(DbContextOptions<VideoPlayerDbContext> options) : DbContext(options)
    {
        public DbSet<Video> Videos { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
