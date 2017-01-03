using System.Diagnostics;
using System.Drawing;
using ImageFinder.SimilarityChecks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageFinder.Tests
{
    [TestClass]
    public class EqualityScanPositionFilterTests
    {
        [TestMethod]
        public void Calc_Sum()
        {
            var fragment = BitmapVisualObject.Factory.Create(new Bitmap(10, 10));
            var mainImage = BitmapVisualObject.Factory.Create(new Bitmap(20, 20));

            var track = new Stopwatch();
            var filter = new EqualityScanPositionFilter(new UnsafeColorSumPaletteCalculator());

            track.Restart();
            var positions = filter.Find(mainImage, fragment);
            track.Stop();

            System.Diagnostics.Debug.WriteLine(track.Elapsed);

            Assert.IsNotNull(positions);
        }
    }
}
