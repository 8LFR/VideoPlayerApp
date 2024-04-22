using Microsoft.AspNetCore.Components.Forms;
using Microsoft.WindowsAPICodePack.Shell;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace VideoPlayerAPI.BusinessLogic.Videos.Services
{
    public class ThumbnailService
    {
        private readonly ILogger<ThumbnailService> _logger;

        public ThumbnailService(ILogger<ThumbnailService> logger)
        {
            _logger = logger;
        }

        public async Task GenerateThumbnailAsync(string filePath)
        {
            var image = Image.FromFile(filePath);
            var ratio = image.Width / image.Height;

            var thumbnail = ResizeImage(image, 150, 150 * ratio);

            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var imageFilePath = Path.Combine(Path.GetDirectoryName(filePath), fileName);

            using var ms = new MemoryStream();

            var xd = imageFilePath + ".png";

            thumbnail.Save(xd);

        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using var graphics = Graphics.FromImage(destImage);

            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using var wrapMode = new ImageAttributes();

            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
            graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);

            return destImage;
        }
    }
}
