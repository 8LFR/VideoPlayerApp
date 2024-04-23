using Microsoft.WindowsAPICodePack.Shell;
using VideoPlayerAPI.Image;

namespace VideoPlayerAPI.BusinessLogic.Videos.Services
{
    public interface IThumbnailService
    {
        Task GenerateThumbnailAsync(string filePath);
    }

    public class ThumbnailService(ILogger<ThumbnailService> logger, IImageService imageService) : IThumbnailService
    {
        private readonly ILogger<ThumbnailService> _logger = logger;
        private readonly IImageService _imageService = imageService;

        public async Task GenerateThumbnailAsync(string filePath)
        {
            try
            {
                var image = GetImage(filePath);

                var resizedImage = _imageService.Resize(image, new Size(640, 360));

                var directoryPath = Directory.GetParent(filePath).ToString();
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var imagePath = Path.Combine(directoryPath, $"{fileName}.{resizedImage.ImageType}");

                using var stream = new FileStream(imagePath, FileMode.Create);
                await stream.WriteAsync(resizedImage.Bytes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate thumbnail for file: {FilePath}", filePath);
                throw;
            }
        }

        private static ImageData GetImage(string fileName)
        {
            var shellFile = ShellFile.FromFilePath(fileName);
            var bm = shellFile.Thumbnail.Bitmap;

            using var ms = new MemoryStream();
            bm.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Flush();

            var image = new ImageData()
            {
                Bytes = ms.ToArray(),
                ImageType = "png"
            };

            return image;
        }
    }
}
