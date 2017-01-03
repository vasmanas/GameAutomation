using System.Drawing;

namespace ImageFinder.SimilarityChecks
{
    public interface IPaletteCalculator<TV> where TV : VisualObject
    {
        ColorSum Calc(TV img);

        ColorSum Calc(TV img, Rectangle rectangle);
    }
}
