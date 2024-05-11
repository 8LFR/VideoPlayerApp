namespace VideoPlayer.Web.Data;

public class VideoSeedDataModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Source { get; set; }
    public Guid RequestedById { get; set; }
}
