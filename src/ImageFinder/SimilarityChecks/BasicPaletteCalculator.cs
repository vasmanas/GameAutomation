using System;
using System.Drawing;

namespace ImageFinder.SimilarityChecks
{
    public abstract class BasicPaletteCalculator : IPaletteCalculator<BitmapVisualObject>
    {
        public virtual ColorSum Calc(BitmapVisualObject img)
        {
            if (img == null)
            {
                throw new ArgumentNullException("img");
            }

            return this.Calc(img, new Rectangle(0, 0, img.Image.Width, img.Image.Height));
        }

        public abstract ColorSum Calc(BitmapVisualObject img, Rectangle rectangle);
    }
}
