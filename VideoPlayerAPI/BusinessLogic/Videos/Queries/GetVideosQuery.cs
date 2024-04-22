using MediatR;
using Microsoft.EntityFrameworkCore;
using VideoPlayerAPI.Models;

namespace VideoPlayerAPI.BusinessLogic.Videos.Queries
{
    public class GetVideosQuery : IRequest<IEnumerable<Video>>
    {
    }

    internal class GetVideosQueryHandler(VideoPlayerDbContext dbContext, IWebHostEnvironment environment) : IRequestHandler<GetVideosQuery, IEnumerable<Video>>
    {
        private readonly VideoPlayerDbContext _dbContext = dbContext;
        private readonly IWebHostEnvironment _environment = environment;

        public async Task<IEnumerable<Video>> Handle(GetVideosQuery query, CancellationToken cancellationToken)
        {
            var videos = await _dbContext.Videos.ToListAsync();
            foreach(var video in videos)
            {
                video.FilePathOrUrl = Path.Combine(_environment.WebRootPath, "uploads", video.FilePathOrUrl);
            }

            return videos;
        }
    }
}
