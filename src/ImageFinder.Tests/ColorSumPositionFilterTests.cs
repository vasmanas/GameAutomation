using System.Drawing;
using ImageFinder.SimilarityChecks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageFinder.Tests
{
    [TestClass]
    public class ColorSumPositionFilterTests
    {
        [TestMethod]
        public void Find_Positions_24()
        {
            var searchArea = BitmapVisualObject.Factory.Create("C:\\work\\Examples\\GameAutomation\\base.bmp");
            var house = BitmapVisualObject.Factory.Create("C:\\work\\Examples\\GameAutomation\\house.bmp", Color.FromArgb(255, 255, 174, 201));

            var filter = new ColorSumPositionFilter(new UnsafeColorSumPaletteCalculator(), new UnsafeColorSumPaletteCalculator(), 10);

            var results = filter.Find(searchArea, house, null);

            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void Find_Positions_32()
        {
            var searchArea = BitmapVisualObject.Factory.Create("C:\\work\\Examples\\GameAutomation\\base32.bmp");
            var house = BitmapVisualObject.Factory.Create("C:\\work\\Examples\\GameAutomation\\house32.bmp", Color.FromArgb(255, 255, 174, 201));

            var filter = new ColorSumPositionFilter(new UnsafeColorSumPaletteCalculator(), new UnsafeColorSumPaletteCalculator(), 10);

            var results = filter.Find(searchArea, house, null);

            Assert.IsNotNull(results);
        }
    }
}

