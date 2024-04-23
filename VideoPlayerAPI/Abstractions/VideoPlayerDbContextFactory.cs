using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VideoPlayerAPI.Abstractions
{
    public class VideoPlayerDbContextFactory : IDesignTimeDbContextFactory<VideoPlayerDbContext>
    {
        public VideoPlayerDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<VideoPlayerDbContext>();
            var connectionString = configuration.GetConnectionString("VideoPlayerConnectionString");
            optionsBuilder.UseSqlServer(connectionString);

            return new VideoPlayerDbContext(optionsBuilder.Options);
        }
    }
}
