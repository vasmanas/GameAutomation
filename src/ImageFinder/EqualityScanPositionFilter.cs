using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using ImageFinder.SimilarityChecks;

namespace ImageFinder
{
    public class EqualityScanPositionFilter : IPositionFilter<BitmapVisualObject>
    {
        private double accuracyPercent;

        private IPaletteCalculator<BitmapVisualObject> colorPixelCalc;

        public EqualityScanPositionFilter(IPaletteCalculator<BitmapVisualObject> colorPixelCalc, double accuracyPercent = 100)
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

        public Rectangle[] Find(BitmapVisualObject plain, BitmapVisualObject fragment, Rectangle[] possibleOccurrences = null)
        {
            if (plain == null)
            {
                throw new ArgumentNullException("plain");
            }

            if (fragment == null)
            {
                throw new ArgumentNullException("fragment");
            }
            
            var maxMatchingPoints = this.CountPixels(fragment, this.colorPixelCalc);
            var result = new List<Rectangle>();

            for (var x = plain.Image.Width - 2 + fragment.Image.Width; x >= 0; x--)
            {
                for (var y = plain.Image.Height - 2 + fragment.Image.Height; y >= 0; y--)
                {
                    var fragmentArea = this.CalcFragmentArea(x, y, fragment.Image.Width, fragment.Image.Height, plain.Image.Width, plain.Image.Height);
                    var plainArea = this.CalcMainArea(x, y, fragment.Image.Width, fragment.Image.Height, plain.Image.Width, plain.Image.Height);
                    
                    var matchinPoints = this.Compare(fragment.Image, fragmentArea, plain.Image, plainArea, fragment.Transparent);

                    if ((matchinPoints > 0) && (((matchinPoints * 100.0) / maxMatchingPoints) >= this.accuracyPercent))
                    {
                        result.Add(plainArea);
                    }
                }
            }

            return result.ToArray();
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

        private unsafe int Compare(Bitmap fImage, Rectangle fArea, Bitmap mImage, Rectangle mArea, Color transparency)
        {
            // TODO: Suskaidyti i sulyginimo ir plotu iskaiciavimo funkcijas

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
                    for (var currY = 0; currY < fImageData.Height; currY++)
                    {
                        byte* frow = fScan0 + (currY * fStride);
                        byte* mrow = mScan0 + (currY * mStride);

                        for (var currX = 0; currX < fImageData.Width; currX++)
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

        private Rectangle CalcFragmentArea(int cX, int cY, int fWidth, int fHeight, int pWidth, int pHeight)
        {
            return
                new Rectangle(
                    cX < fWidth - 1 ? fWidth - cX - 1 : 0,
                    cY < fHeight - 1 ? fHeight - cY - 1 : 0,
                    cX < fWidth - 1 ? cX + 1 : (cX < pWidth ? fWidth : pWidth + fWidth - 1 - cX),
                    cY < fHeight - 1 ? cY + 1 : (cY < pHeight ? fHeight : pHeight + fHeight - 1 - cY));
        }
        private Rectangle CalcMainArea(int cX, int cY, int fWidth, int fHeight, int pWidth, int pHeight)
        {
            return
                new Rectangle(
                    cX < fWidth ? 0 : cX - fWidth,
                    cY < fHeight ? 0 : cY - fHeight,
                    cX < fWidth - 1 ? cX + 1 : (cX < pWidth ? fWidth : pWidth + fWidth - 1 - cX),
                    cY < fHeight - 1 ? cY + 1 : (cY < pHeight ? fHeight : pHeight + fHeight - 1 - cY));
        }
    }
}
