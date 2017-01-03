using System;
using System.Drawing;

namespace ImageFinder.SimilarityChecks
{
    public class ColorSumSimilarityCheck : ISimilarityCheck
    {
        private IPaletteCalculator<BitmapVisualObject> mainCalculator;

        private IPaletteCalculator<BitmapVisualObject> fragmentCalculator;

        private double accuracyPercent;

        private ColorSum prevFragmentColorSum;

        private BitmapVisualObject prevFragment;

        private Rectangle prevFragmentArea;

        public ColorSumSimilarityCheck(IPaletteCalculator<BitmapVisualObject> mainCalculator, IPaletteCalculator<BitmapVisualObject> fragmentCalculator, double accuracyPercent = 100)
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

        public bool Compare(BitmapVisualObject mainImage, Rectangle mainArea, BitmapVisualObject fragment, Rectangle fragmentArea)
        {
            if (mainImage == null)
            {
                throw new ArgumentNullException("mainImage");
            }

            if (fragment == null)
            {
                throw new ArgumentNullException("fragment");
            }

            var mainPalette = this.mainCalculator.Calc(mainImage, mainArea);

            // Jei fragment ir fragmentArea nesikeite, tai perskaiciuoti nereikia
            if (this.prevFragmentColorSum == null || !this.prevFragment.Equals(fragment) || !this.prevFragmentArea.Equals(fragmentArea))
            {
                this.prevFragmentColorSum = this.fragmentCalculator.Calc(fragment);
                this.prevFragment = fragment;
                this.prevFragmentArea = fragmentArea;
            }

            var matchSum = ColorSum.Match(this.prevFragmentColorSum, mainPalette);

            return (((matchSum * 100.0) / this.prevFragmentColorSum.Total()) >= this.accuracyPercent);
        }
    }
}
