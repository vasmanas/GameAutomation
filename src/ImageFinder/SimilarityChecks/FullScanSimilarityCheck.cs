using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageFinder.SimilarityChecks
{
    public class FullScanSimilarityCheck : ISimilarityCheck
    {
        private double accuracyPercent;

        private IPaletteCalculator<BitmapVisualObject> colorPixelCalc;

        private int prevMaxMatchingPoints = -1;

        private BitmapVisualObject prevFragment;

        private Rectangle prevFragmentArea;

        public FullScanSimilarityCheck(IPaletteCalculator<BitmapVisualObject> colorPixelCalc, double accuracyPercent = 100)
        {
            if (accuracyPercent < 0 || accuracyPercent > 100)
            {
                throw new ArgumentOutOfRangeException("accuracyPercent", accuracyPercent, "Percent value can be in range between 0 and 100");
            }

            if (colorPixelCalc == null)
            {
                throw new ArgumentNullException("colorPixelCalc");
            }

            this.accuracyPercent = accuracyPercent;
            this.colorPixelCalc = colorPixelCalc;
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

            // Jei fragment ir fragmentArea nesikeite, tai perskaiciuoti nereikia
            if (this.prevMaxMatchingPoints == -1 || !this.prevFragment.Equals(fragment) || !this.prevFragmentArea.Equals(fragmentArea))
            {
                this.prevMaxMatchingPoints = this.CountPixels(fragment, this.colorPixelCalc);
                this.prevFragment = fragment;
                this.prevFragmentArea = fragmentArea;
            }

            var matchinPoints = this.CountMatches(fragment.Image, fragmentArea, mainImage.Image, mainArea, fragment.Transparent);

            return ((matchinPoints > 0) && (((matchinPoints * 100.0) / this.prevMaxMatchingPoints) >= this.accuracyPercent));
        }

        private int CountPixels(BitmapVisualObject img, IPaletteCalculator<BitmapVisualObject> cal)
        {
            var colors = cal.Calc(img);

            var result = 0;
            foreach (var color in colors)
            {
                result += color.Value;
            }

            return result;
        }

        private unsafe int CountMatches(Bitmap fImage, Rectangle fArea, Bitmap mImage, Rectangle mArea, Color transparency)
        {
            var fImageData = fImage.LockBits(fArea, ImageLockMode.ReadOnly, fImage.PixelFormat);

            try
            {
                var mImageData = mImage.LockBits(mArea, ImageLockMode.ReadOnly, mImage.PixelFormat);

                try
                {
                    var fbpp = fImageData.Stride / fImage.Width;
                    byte* fScan0 = (byte*)fImageData.Scan0.ToPointer();
                    int fStride = fImageData.Stride;

                    var mbpp = mImageData.Stride / mImage.Width;
                    byte* mScan0 = (byte*)mImageData.Scan0.ToPointer();
                    var mStride = mImageData.Stride;

                    var matches = 0;
                    for (var currY = Math.Min(mImageData.Height, fImageData.Height) - 1; currY >= 0; currY--)
                    {
                        byte* frow = fScan0 + (currY * fStride);
                        byte* mrow = mScan0 + (currY * mStride);

                        for (var currX = Math.Min(mImageData.Width, fImageData.Width) - 1; currX >= 0; currX--)
                        {
                            var fbIndex = fbpp * currX;

                            // TODO: galima kazkiek paspartinti atsisakant sio patikrinimo
                            // kai zinome, kad fbpp nera 4
                            if (fbpp == 4 && frow[fbIndex + 3] == 0)
                            {
                                continue;
                            }

                            var fblue = frow[fbIndex];
                            var fgreen = frow[fbIndex + 1];
                            var fred = frow[fbIndex + 2];

                            // TODO: galima kazkiek paspartinti atsisakant sio patikrinimo
                            // kai zinome, kad transpacency spalva nepateikta
                            if (!transparency.IsEmpty && fblue == transparency.B && fgreen == transparency.G && fred == transparency.R)
                            {
                                continue;
                            }

                            var mbIndex = mbpp * currX;

                            if (fblue == mrow[mbIndex] && fgreen == mrow[mbIndex + 1] && fred == mrow[mbIndex + 2])
                            {
                                matches += 1;
                            }
                        }
                    }

                    return matches;
                }
                finally
                {
                    mImage.UnlockBits(mImageData);
                }
            }
            finally
            {
                fImage.UnlockBits(fImageData);
            }
        }
    }
}
