using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageFinder.SimilarityChecks
{
    public class UnsafeColorSumPaletteCalculator : BasicPaletteCalculator
    {
        public override unsafe ColorSum Calc(BitmapVisualObject img, Rectangle rectangle)
        {
            if (img == null)
            {
                throw new ArgumentNullException("img");
            }

            var im = img.Image;

            var calcArea = rectangle;
            if (calcArea == null)
            {
                calcArea = new Rectangle(0, 0, im.Width, im.Height);
            }

            var csum = new ColorSum();

            var imageData = im.LockBits(calcArea, ImageLockMode.ReadOnly, im.PixelFormat);

            try
            {
                var bpp = imageData.Stride / im.Width;

                byte* scan0 = (byte*)imageData.Scan0.ToPointer();
                int stride = imageData.Stride;
                
                for (var currY = 0; currY < imageData.Height; currY++)
                {
                    byte* row = scan0 + (currY * stride);

                    for (var currX = 0; currX < imageData.Width; currX++)
                    {
                        int bIndex = currX * bpp;
                        int gIndex = bIndex + 1;
                        int rIndex = bIndex + 2;

                        byte pixelR = row[rIndex];
                        byte pixelG = row[gIndex];
                        byte pixelB = row[bIndex];
                        
                        // Opacity byte
                        if (bpp == 4 && row[bIndex + 3] == 0)
                        {
                            continue;
                        }

                        var color = Color.FromArgb(255, pixelR, pixelG, pixelB);

                        csum.Add(color);
                    }
                }
            }
            finally
            {
                im.UnlockBits(imageData);
            }
            
            if (img.HasTransparentColor())
            {
                csum.Remove(img.Transparent);
            }
            
            return csum;
        }
    }
}
