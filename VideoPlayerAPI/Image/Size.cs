using SkiaSharp;
using System.ComponentModel;

namespace VideoPlayerAPI.Image
{
    public struct Size(float width, float height)
    {
        private SKSize _size = new(width, height);

        public bool IsZero => Width == 0 && Height == 0;

        [DefaultValue(0d)]
        public float Width
        {
            get => _size.Width;
            set
            {
                if (float.IsNaN(value))
                {
                    throw new ArgumentException("NaN is not a valid value for width");
                }

                _size.Width = value;
            }
        }

        [DefaultValue(0d)]
        public float Height
        {
            get => _size.Height;
            set
            {
                if (float.IsNaN(value))
                {
                    throw new ArgumentException("NaN is not a valid value for height");
                }

                _size.Height = value;
            }
        }
    }
}
