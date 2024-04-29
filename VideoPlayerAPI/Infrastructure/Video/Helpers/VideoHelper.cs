using System.Diagnostics;
using System.Globalization;
using VideoPlayerAPI.Infrastructure.Image.Models;

namespace VideoPlayerAPI.Infrastructure.Video.Helpers;

public interface IVideoHelper
{
    Task<ImageData> GenerateThumbnailAsync(Guid videoId, VideoData videoData, string resolution = "640x360");
    Task<TimeSpan> GetVideoDurationAsync(Guid videoId, VideoData videoData);
}

public class VideoHelper : IVideoHelper
{
    private const string FfmpegContainerName = "ffmpeg-container";

    public async Task<ImageData> GenerateThumbnailAsync(Guid videoId, VideoData videoData, string resolution = "640x360")
    {
        var tempVideoPath = Path.Combine(Path.GetTempPath(), $"{videoId}.mp4");

        try
        {
            File.WriteAllBytes(tempVideoPath, videoData.Bytes);

            var tempThumbnailPath = Path.Combine(Path.GetTempPath(), $"{videoId}.png");

            var ffmpegProcess = new Process();

            ffmpegProcess.StartInfo.FileName = "docker-compose";
            ffmpegProcess.StartInfo.Arguments = $"run --rm -v {Path.GetTempPath()}:/temp {FfmpegContainerName} -i /temp/{videoId}.mp4 -vf scale={resolution} -ss 00:00:05 -vframes 1 /temp/{videoId}.png";
            ffmpegProcess.StartInfo.UseShellExecute = false;

            ffmpegProcess.Start();

            await ffmpegProcess.WaitForExitAsync();

            ffmpegProcess.Close();

            if (File.Exists(tempThumbnailPath))
            {
                var thumbnailBytes = File.ReadAllBytes(tempThumbnailPath);

                return new ImageData()
                {
                    ImageType = "png",
                    Bytes = thumbnailBytes
                };
            }
            else
            {
                throw new Exception("Thumbnail generation failed");
            }
        }
        finally
        {
            if (File.Exists(tempVideoPath))
            {
                File.Delete(tempVideoPath);
            }
        }
    }

    public async Task<TimeSpan> GetVideoDurationAsync(Guid videoId, VideoData videoData)
    {
        var tempVideoPath = Path.Combine(Path.GetTempPath(), $"{videoId}.mp4");

        try
        {
            File.WriteAllBytes(tempVideoPath, videoData.Bytes);

            var ffprobeProcess = new Process();

            ffprobeProcess.StartInfo.FileName = "docker-compose";
            ffprobeProcess.StartInfo.Arguments = $"run --rm -v {Path.GetTempPath()}:/temp --entrypoint=ffprobe {FfmpegContainerName} -v error -select_streams v:0 -show_entries stream=duration -of default=noprint_wrappers=1:nokey=1 /temp/{videoId}.mp4";
            ffprobeProcess.StartInfo.UseShellExecute = false;
            ffprobeProcess.StartInfo.RedirectStandardOutput = true;

            ffprobeProcess.Start();

            var duration = await ffprobeProcess.StandardOutput.ReadToEndAsync();

            var durationString = duration.ToString().Trim();

            ffprobeProcess.WaitForExit();

            if (double.TryParse(durationString, CultureInfo.InvariantCulture, out double seconds))
            {
                return TimeSpan.FromSeconds(seconds);
            }
            else
            {
                throw new Exception("Failed to parse video duration");
            }
        }
        finally
        {
            if (File.Exists(tempVideoPath))
            {
                File.Delete(tempVideoPath);
            }
        }
    }
}
