using MediatR;
using Microsoft.EntityFrameworkCore;
using VideoPlayerAPI.Models;

namespace VideoPlayerAPI.BusinessLogic.Videos.Queries
{
    public class GetVideosQuery : IRequest<IEnumerable<Video>>
    {
    }

    internal class GetVideosQueryHandler(VideoPlayerDbContext dbContext) : IRequestHandler<GetVideosQuery, IEnumerable<Video>>
    {
        private readonly VideoPlayerDbContext _dbContext = dbContext;

        public async Task<IEnumerable<Video>> Handle(GetVideosQuery query, CancellationToken cancellationToken)
        {
            var videos = await _dbContext.Videos.ToListAsync();

            return videos;
        }
    }
}
