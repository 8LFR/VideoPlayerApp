using MediatR;
using VideoPlayerAPI.Models;

namespace VideoPlayerAPI.BusinessLogic.Videos.Queries
{
    public class GetVideoByIdQuery : IRequest<Video>
    {
        public int VideoId { get; set; }
    }

    internal class GetVideoByIdQueryHandler(VideoPlayerDbContext dbContext) : IRequestHandler<GetVideoByIdQuery, Video>
    {
        private readonly VideoPlayerDbContext _dbContext = dbContext;

        public async Task<Video> Handle(GetVideoByIdQuery query, CancellationToken cancellationToken)
        {
            var video = await _dbContext.Videos.FindAsync(query.VideoId);

            return video ?? throw new Exception("Video not found");
        }
    }
}
