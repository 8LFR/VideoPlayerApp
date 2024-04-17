using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace VideoPlayerAPI.BusinessLogic.Videos.Extensions
{
    internal static class VideoExtensions
    {
        public static TimeSpan GetVideoDuration(string filePath)
        {
            using var shell = ShellObject.FromParsingName(filePath);

            IShellProperty prop = shell.Properties.System.Media.Duration;
            var t = (ulong)prop.ValueAsObject;
            return TimeSpan.FromTicks((long)t);
        }
    }
}
