namespace VideoPlayerAPI.BusinessLogic.Videos.Mappers;

internal static class VideoMapper
{
    public static Models.Video ToModel(this Abstractions.Models.Video video)
    {
        return new Models.Video
        {
            Id = video.Id
        };
    }
}
