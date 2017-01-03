using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageFinder.Tests
{
    [TestClass]
    public class VisualObjectTests
    {
        [TestMethod]
        public void Load_Image()
        {
            var img = BitmapVisualObject.Factory.Create("C:\\work\\Examples\\GameAutomation\\parts.bmp");

            Assert.IsNotNull(img);
        }
    }
}
