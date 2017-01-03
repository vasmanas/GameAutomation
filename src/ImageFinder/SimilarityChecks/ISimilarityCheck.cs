using System.Drawing;

namespace ImageFinder.SimilarityChecks
{
    public interface ISimilarityCheck
    {
        bool Compare(BitmapVisualObject mainImage, Rectangle mainArea, BitmapVisualObject fragment, Rectangle fragmentArea);
    }
}
