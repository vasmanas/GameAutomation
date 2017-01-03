using System;
using System.Collections.Generic;
using System.Drawing;
using ImageFinder.SimilarityChecks;

namespace ImageFinder.AreaScanners
{
    public class FragmentGridScanner : IAreaScanner
    {
        private ISimilarityCheck check;

        public FragmentGridScanner(ISimilarityCheck check)
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
                throw new ArgumentNullException("plain");
            }

            if (fragment == null)
            {
                throw new ArgumentNullException("fragment");
            }

            var mwidth = mainImage.Image.Width;
            var mheight = mainImage.Image.Height;
            var fwidth = fragment.Image.Width;
            var fheight = fragment.Image.Height;

            var widthRatio = (mwidth / fwidth) + (mwidth % fwidth > 0 ? 1 : 0);
            var heightRatio = (mheight / fheight) + (mheight % fheight > 0 ? 1 : 0);

            // WARN: Dabar side, ceiling ir angle duoda neteisinga fragmento plota
            var fragmentArea = new Rectangle(0, 0, fwidth, fheight);

            var result = new List<Rectangle>();
            
            for (var x = 0; x < widthRatio; x++)
            {
                for (var y = 0; y < heightRatio; y++)
                {
                    var mainImageArea = new Rectangle(
                        x * fwidth,
                        y * fheight,
                        x == (widthRatio - 1) && (mwidth % fwidth > 0) ? mwidth % fwidth : fwidth,
                        y == (heightRatio - 1) && (mheight % fheight > 0) ? mheight % fheight : fheight);
                    
                    if (this.check.Compare(mainImage, mainImageArea, fragment, fragmentArea))
                    {
                        result.Add(mainImageArea);
                    }
                }
            }

            return result.ToArray();
        }
    }
}
