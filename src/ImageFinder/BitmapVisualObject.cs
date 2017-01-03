using System;
using System.Drawing;

namespace ImageFinder
{
    public class BitmapVisualObject : VisualObject
    {
        public BitmapVisualObject(Bitmap image)
            : this (image, Color.Empty)
        {
        }

        public BitmapVisualObject(Bitmap image, Color transparent)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }

            this.Image = image;
            this.Transparent = transparent;
        }

        public Bitmap Image { get; private set; }

        public Color Transparent { get; private set; }

        public bool HasTransparentColor()
        {
            return !this.Transparent.IsEmpty;
        }

        public static class Factory
        {
            public static BitmapVisualObject Create(string filePath)
            {
                return Create(filePath, Color.Empty);
            }

            public static BitmapVisualObject Create(string filePath, Color transparency)
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    throw new ArgumentNullException("filePath");
                }

                var image = (Bitmap)Bitmap.FromFile(filePath);

                return Create(image, transparency);
            }

            public static BitmapVisualObject Create(Bitmap image)
            {
                return Create(image, Color.Empty);
            }

            public static BitmapVisualObject Create(Bitmap image, Color transparency)
            {
                return new BitmapVisualObject(image, transparency);
            }
        }
    }
}
