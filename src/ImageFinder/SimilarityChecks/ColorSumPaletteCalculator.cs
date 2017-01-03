using System;
using System.Drawing;

namespace ImageFinder.SimilarityChecks
{
    public class ColorSumPaletteCalculator : BasicPaletteCalculator
    {
        public override ColorSum Calc(BitmapVisualObject img, Rectangle rectangle)
        {
            if (img == null)
            {
                throw new ArgumentNullException("img");
            }
            
            var im = img.Image;

            var calcArea = rectangle;
            if (calcArea == Rectangle.Empty)
            {
                calcArea = new Rectangle(0, 0, im.Width, im.Height);
            }

            var csum = new ColorSum();

            var bottom = calcArea.Top + calcArea.Height;
            var left = calcArea.Left;
            var right = calcArea.Left + calcArea.Width;

            for (var y = calcArea.Top; y < bottom; y++)
            {
                for (var x = left; x < right; x++)
                {
                    var color = im.GetPixel(x, y);

                    if (color.A != 0)
                    {
                        csum.Add(color);
                    }
                }
            }

            if (img.HasTransparentColor())
            {
                csum.Remove(img.Transparent);
            }

            return csum;
        }
    }
}
