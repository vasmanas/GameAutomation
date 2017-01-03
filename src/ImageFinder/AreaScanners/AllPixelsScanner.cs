using System;
using System.Collections.Generic;
using System.Drawing;
using ImageFinder.SimilarityChecks;

namespace ImageFinder.AreaScanners
{
    public class AllPixelsScanner : IAreaScanner
    {
        private ISimilarityCheck check;

        public AllPixelsScanner(ISimilarityCheck check)
        {
            if (check == null)
            {
                throw new ArgumentNullException("check");
            }

            this.check = check;
        }

        public Rectangle[] Scan(BitmapVisualObject mainImage, BitmapVisualObject fragment, Rectangle[] possibleOccurrences = null)
        {
            if (mainImage == null)
            {
                throw new ArgumentNullException("mainImage");
            }

            if (fragment == null)
            {
                throw new ArgumentNullException("fragment");
            }

            var mwidth = mainImage.Image.Width;
            var mheight = mainImage.Image.Height;
            var fwidth = fragment.Image.Width;
            var fheight = fragment.Image.Height;

            var result = new List<Rectangle>();

            // fragment rectangle visada vienodas
            this.ScanCenter(result, mainImage, mwidth, mheight, fragment, fwidth, fheight);

            // perskaiciuojame kai keiciasi x
            this.ScanCeiling(result, mainImage, mwidth, mheight, fragment, fwidth, fheight, 0, fheight - 2);
            this.ScanCeiling(result, mainImage, mwidth, mheight, fragment, fwidth, fheight, mheight, mheight + fheight - 2);

            // perskaiciuojame kai keiciasi y
            this.ScanSide(result, mainImage, mwidth, mheight, fragment, fwidth, fheight, 0, fwidth - 2);
            this.ScanSide(result, mainImage, mwidth, mheight, fragment, fwidth, fheight, mwidth, mwidth + fwidth - 2);

            // visada perskaiciuojame
            this.ScanCorner(result, mainImage, mwidth, mheight, fragment, fwidth, fheight, 0, fwidth - 2, 0, fheight - 2);
            this.ScanCorner(result, mainImage, mwidth, mheight, fragment, fwidth, fheight, 0, fwidth - 2, mheight, mheight + fheight - 2);
            this.ScanCorner(result, mainImage, mwidth, mheight, fragment, fwidth, fheight, mwidth, mwidth + fwidth - 2, 0, fheight - 2);
            this.ScanCorner(result, mainImage, mwidth, mheight, fragment, fwidth, fheight, mwidth, mwidth + fwidth - 2, mheight, mheight + fheight - 2);

            return result.ToArray();
        }

        private void ScanCeiling(
            List<Rectangle> areas,
            BitmapVisualObject mi,
            int mw,
            int mh,
            BitmapVisualObject fi,
            int fw,
            int fh,
            int yfrom,
            int yto)
        {
            for (var y = yfrom; y <= yto; y++)
            {
                var fragmentArea = this.CalcFragmentArea(mw - 1, y, mw, mh, fw, fh);

                for (var x = mw - 1; x >= fw - 1; x--)
                {
                    var mainImageArea = this.CalcMainArea(x, y, mw, mh, fw, fh);

                    if (this.check.Compare(mi, mainImageArea, fi, fragmentArea))
                    {
                        areas.Add(mainImageArea);
                    }
                }
            }
        }

        private void ScanSide(
            List<Rectangle> areas,
            BitmapVisualObject mi,
            int mw,
            int mh,
            BitmapVisualObject fi,
            int fw,
            int fh,
            int xfrom,
            int xto)
        {
            for (var x = xfrom; x <= xto; x++)
            {
                var fragmentArea = this.CalcFragmentArea(x, mh - 1, mw, mh, fw, fh);

                for (var y = mh - 1; y >= fh - 1; y--)
                {
                    var mainImageArea = this.CalcMainArea(x, y, mw, mh, fw, fh);

                    if (this.check.Compare(mi, mainImageArea, fi, fragmentArea))
                    {
                        areas.Add(mainImageArea);
                    }
                }
            }
        }

        private void ScanCorner(
            List<Rectangle> areas,
            BitmapVisualObject mi,
            int mw,
            int mh,
            BitmapVisualObject fi,
            int fw,
            int fh,
            int xfrom,
            int xto,
            int yfrom,
            int yto)
        {
            for (var x = xfrom; x <= xto; x++)
            {
                for (var y = yfrom; y <= yto; y++)
                {
                    var fragmentArea = this.CalcFragmentArea(x, y, mw, mh, fw, fh);
                    var mainImageArea = this.CalcMainArea(x, y, mw, mh, fw, fh);

                    if (this.check.Compare(mi, mainImageArea, fi, fragmentArea))
                    {
                        areas.Add(mainImageArea);
                    }
                }
            }
        }

        private void ScanCenter(List<Rectangle> areas, BitmapVisualObject mi, int mw, int mh, BitmapVisualObject fi, int fw, int fh)
        {
            var fragmentArea = this.CalcFragmentArea(mw - 1, mh - 1, mw, mh, fw, fh);

            for (var x = mw - 1; x >= fw - 1; x--)
            {
                for (var y = mh - 1; y >= fh - 1; y--)
                {
                    var mainImageArea = this.CalcMainArea(x, y, mw, mh, fw, fh);

                    if (this.check.Compare(mi, mainImageArea, fi, fragmentArea))
                    {
                        areas.Add(mainImageArea);
                    }
                }
            }
        }

        private Rectangle CalcFragmentArea(int cX, int cY, int pWidth, int pHeight, int fWidth, int fHeight)
        {
            return
                new Rectangle(
                    cX < fWidth - 1 ? fWidth - cX - 1 : 0,
                    cY < fHeight - 1 ? fHeight - cY - 1 : 0,
                    cX < fWidth - 1 ? cX + 1 : (cX < pWidth ? fWidth : pWidth + fWidth - 1 - cX),
                    cY < fHeight - 1 ? cY + 1 : (cY < pHeight ? fHeight : pHeight + fHeight - 1 - cY));
        }

        private Rectangle CalcMainArea(int cX, int cY, int pWidth, int pHeight, int fWidth, int fHeight)
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
