using System.Drawing;

namespace ImageFinder.AreaScanners
{
    interface IAreaScanner
    {
        Rectangle[] Scan(BitmapVisualObject mainImage, BitmapVisualObject fragment, Rectangle[] possibleOccurrences = null);
    }
}
