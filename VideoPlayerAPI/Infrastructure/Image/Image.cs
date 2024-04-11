using SkiaSharp;

namespace VideoPlayerAPI.Infrastructure.Image
{
    public interface IImage : IDisposable
    {
        float Width { get; }
        float Height { get; }
        IImage Resize(double width, double height);
        void Save(Stream stream, ImageFormat imageFormat);
    }

    public enum ImageFormat
    {
        Png = 0,
        Jpeg = 1,
        Gif = 2
    }

    public class Image(SKImage image) : IImage
    {
        private readonly SKImage _image = image;

        public float Width => _image.Width;
        public float Height => _image.Height;

        public static IImage FromStream(Stream stream)
        {
            stream.Position = 0;

            var image = new Image(SKImage.FromEncodedData(stream));

            stream.Position = 0;

            return image;
        }

        public static IImage FromByteArray(byte[] bytes)
        {
            return new Image(SKImage.FromEncodedData(bytes));
        }

        public void Dispose()
        {
            _image.Dispose();
        }

        public IImage Resize(double width, double height)
        {
            return FromByteArray(Resize((int)width, (int)height, SKFilterQuality.High));
        }

        public void Save(Stream stream, ImageFormat imageFormat)
        {
            var image = _image.Encode(Convert(imageFormat), 100);
            image.SaveTo(stream);
        }

        private byte[] Resize(
            int maxWidth,
            int maxHeight,
            SKFilterQuality quality = SKFilterQuality.Medium)
        {
            using var sourceBitmap = SKBitmap.FromImage(_image);

            var height = Math.Min(maxHeight, sourceBitmap.Height);
            var width = Math.Min(maxWidth, sourceBitmap.Width);

            using var scaledBitmap = sourceBitmap.Resize(new SKImageInfo(width, height), quality);
            using var scaledImage = SKImage.FromBitmap(scaledBitmap);
            using var data = scaledImage.Encode();

            return data.ToArray();
        }

        private static SKEncodedImageFormat Convert(ImageFormat imageFormat)
        {
            return imageFormat switch
            {
                ImageFormat.Png => SKEncodedImageFormat.Png,
                ImageFormat.Jpeg => SKEncodedImageFormat.Jpeg,
                ImageFormat.Gif => SKEncodedImageFormat.Gif,
                _ => throw new ArgumentOutOfRangeException(nameof(imageFormat), imageFormat, null),
            };
        }
    }
}
