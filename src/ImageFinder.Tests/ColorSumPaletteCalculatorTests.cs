using System.Diagnostics;
using System.Drawing;
using ImageFinder.SimilarityChecks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageFinder.Tests
{
    [TestClass]
    public class ColorSumPaletteCalculatorTests
    {
        [TestMethod]
        public void Calc_Sum()
        {
            var img = BitmapVisualObject.Factory.Create("C:\\work\\Examples\\GameAutomation\\base.bmp");

            var calc = new ColorSumPaletteCalculator();
            var track = new Stopwatch();

            track.Restart();
            var vector = calc.Calc(img, new Rectangle(0, 0, 10, 10));
            track.Stop();

            System.Diagnostics.Debug.WriteLine(track.Elapsed);

            Assert.IsNotNull(vector);
        }

        [TestMethod]
        public void Calc_Unsafe_Sum()
        {
            var img = BitmapVisualObject.Factory.Create("C:\\work\\Examples\\GameAutomation\\base.bmp");

            var calc = new UnsafeColorSumPaletteCalculator();
            var track = new Stopwatch();

            track.Restart();
            var vector = calc.Calc(img, new Rectangle(20, 20, 10, 10));
            track.Stop();

            System.Diagnostics.Debug.WriteLine(track.Elapsed);

            Assert.IsNotNull(vector);
        }
    }
}
