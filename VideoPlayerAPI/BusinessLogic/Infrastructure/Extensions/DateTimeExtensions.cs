namespace VideoPlayerAPI.BusinessLogic.Infrastructure.Extensions;

public static class DateTimeExtensions
{
    public static string GetCreationInfo(this DateTimeOffset uploadDate)
    {
        var timeSinceCreation = DateTimeOffset.UtcNow - uploadDate;

        if (timeSinceCreation.TotalSeconds < 60)
        {
            return "Just now";
        }
        else if (timeSinceCreation.TotalMinutes < 60)
        {
            var minutes = (int)timeSinceCreation.TotalMinutes;
            return $"{minutes} minute{(minutes == 1 ? "" : "s")} ago";
        }
        else if (timeSinceCreation.TotalHours < 24)
        {
            var hours = (int)timeSinceCreation.TotalHours;
            return $"{hours} hour{(hours == 1 ? "" : "s")} ago";
        }
        else if (timeSinceCreation.TotalDays < 365)
        {
            var days = (int)timeSinceCreation.TotalDays;
            return $"{days} day{(days == 1 ? "" : "s")} ago";
        }
        else
        {
            var years = (int)(timeSinceCreation.TotalDays / 365);
            return $"{years} year{(years == 1 ? "" : "s")} ago";
        }
    }
}
