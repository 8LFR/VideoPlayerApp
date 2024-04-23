namespace VideoPlayerAPI.Image
{
    public interface IImageService
    {
        ImageData Resize(ImageData source, Size requestedSize);
    }

    public class ImageService : IImageService
    {
        public ImageData Resize(ImageData source, Size requestedSize)
        {
            using var sourceImage = Image.FromByteArray(source.Bytes);
            using var resizedImage = sourceImage.Resize(requestedSize.Width, requestedSize.Height);

            return new ImageData
            {
                Bytes = ImageToBytes(resizedImage, ImageFormat.Png),
                ImageType = source.ImageType
            };
        }

        private static byte[] ImageToBytes(IImage sourceImage, ImageFormat imageFormat)
        {
            using var ms = new MemoryStream();
            sourceImage.Save(ms, imageFormat);
            ms.Flush();
            return ms.ToArray();
        }
    }
}
