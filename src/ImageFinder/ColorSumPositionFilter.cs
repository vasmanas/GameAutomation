using System;
using System.Collections.Generic;
using System.Drawing;
using ImageFinder.SimilarityChecks;

namespace ImageFinder
{
    public class ColorSumPositionFilter : IPositionFilter<BitmapVisualObject>
    {
        private IPaletteCalculator<BitmapVisualObject> mainCalculator;

        private IPaletteCalculator<BitmapVisualObject> fragmentCalculator;

        private double accuracyPercent;
        
        public ColorSumPositionFilter(IPaletteCalculator<BitmapVisualObject> mainCalculator, IPaletteCalculator<BitmapVisualObject> fragmentCalculator, double accuracyPercent = 100)
        {
            if (mainCalculator == null)
            {
                throw new ArgumentNullException("mainCalculator");
            }

            if (fragmentCalculator == null)
            {
                throw new ArgumentNullException("fragmentCalculator");
            }

            if (accuracyPercent < 0 || accuracyPercent > 100)
            {
                throw new ArgumentOutOfRangeException("accuracyPercent", accuracyPercent, "Percent value can be in range between 0 and 100");
            }

            this.mainCalculator = mainCalculator;

            this.fragmentCalculator = fragmentCalculator;

            this.accuracyPercent = accuracyPercent;
        }

        public Rectangle[] Find(BitmapVisualObject plain, BitmapVisualObject fragment, Rectangle[] possibleOccurrences)
        {
            if (plain == null)
            {
                throw new ArgumentNullException("plain");
            }

            if (fragment == null)
            {
                throw new ArgumentNullException("fragment");
            }

            var heightRatio = (plain.Image.Height / fragment.Image.Height) + (plain.Image.Height % fragment.Image.Height > 0 ? 1 : 0);
            var widthRatio = (plain.Image.Width / fragment.Image.Width) + (plain.Image.Width % fragment.Image.Width > 0 ? 1 : 0);

            var fragmentPalette = this.fragmentCalculator.Calc(fragment);
            var result = new List<Rectangle>();
            
            for (var x = 0; x < widthRatio; x++)
            {
                for (var y = 0; y < heightRatio; y++)
                {
                    var rec = new Rectangle(
                        x * fragment.Image.Width,
                        y * fragment.Image.Height,
                        x == (widthRatio - 1) && (plain.Image.Width % fragment.Image.Width > 0) ? plain.Image.Width % fragment.Image.Width : fragment.Image.Width,
                        y == (heightRatio - 1) && (plain.Image.Height % fragment.Image.Height > 0) ? plain.Image.Height % fragment.Image.Height : fragment.Image.Height);

                    var partPalette = this.mainCalculator.Calc(plain, rec);

                    var matchSum = ColorSum.Match(fragmentPalette, partPalette);

                    if (((matchSum * 100.0) / fragmentPalette.Total()) >= this.accuracyPercent)
                    {
                        result.Add(rec);
                    }
                }
            }

            return result.ToArray();
        }
    }
}
