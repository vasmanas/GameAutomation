using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ImageFinder;
using ImageFinder.AreaScanners;
using ImageFinder.SimilarityChecks;
using Visualizer.Converters;

namespace Visualizer
{
    /// <summary>
    /// Interaction logic for ImageSearchWindow.xaml
    /// </summary>
    public partial class ImageSearchWindow : Window
    {
        private BitmapConverter bitmapConverter = new BitmapConverter();
        
        private Bitmap mainOriginal;

        private Bitmap fragmentOriginal;

        private System.Windows.Point scrollMousePoint = new System.Windows.Point();
        private double hOff = 1;
        private double vOff = 1;

        public ImageSearchWindow()
        {
            InitializeComponent();
        }

        private void MainImageScroll_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                this.mainOriginal = Bitmap.FromFile(files[0]) as Bitmap;

                this.MainImage.Source = this.bitmapConverter.Convert(this.mainOriginal);

                this.DisplayMessage("Ikeltas pagrindinis paveikliukas");
            }

            e.Effects = DragDropEffects.None;
        }

        private void FragmentImage_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                this.fragmentOriginal = Bitmap.FromFile(files[0]) as Bitmap;

                this.FragmentImage.Source = this.bitmapConverter.Convert(this.fragmentOriginal);

                this.DisplayMessage("Ikeltas fragmento paveikliukas");
            }

            e.Effects = DragDropEffects.None;
        }

        private void CompareEquality_Click(object sender, RoutedEventArgs e)
        {
            if (this.mainOriginal == null)
            {
                this.DisplayMessage("Nera pagrindinio paveiksliuko");

                return;
            }

            if (this.fragmentOriginal == null)
            {
                this.DisplayMessage("Nera fragmento paveiksliuko");

                return;
            }

            int acc;
            if (!int.TryParse(this.CompareEqualityAccuracy.Text, out acc) || acc < 1 || acc > 100)
            {
                this.DisplayMessage("Accuracy must be number between 1 and 100");

                return;
            }

            this.DisplayMessage("Vyksta lyginimas");

            var mainImage = BitmapVisualObject.Factory.Create(this.mainOriginal);
            var fragment = BitmapVisualObject.Factory.Create(this.fragmentOriginal);

            //var filter = new EqualityScanPositionFilter(new UnsafeColorSumPaletteCalculator(), acc);
            //var resultedRectangles = filter.Find(mainImage, fragment, null);

            var scanner = new AllPixelsScanner(new FullScanSimilarityCheck(new UnsafeColorSumPaletteCalculator(), acc));
            var resultedRectangles = scanner.Scan(mainImage, fragment);

            var im = new Bitmap(this.mainOriginal);

            if (resultedRectangles != null && resultedRectangles.Length > 0)
            {
                this.DisplayMessage("Rasta {0} fragmentu", resultedRectangles.Length);

                Graphics.FromImage(im).DrawRectangles(new System.Drawing.Pen(System.Drawing.Color.Red, 1), resultedRectangles);
            }
            else
            {
                this.DisplayMessage("Fragmentu nerasta");
            }

            this.MainImage.Source = this.bitmapConverter.Convert(im);
        }

        private void CompareColorSum_Click(object sender, RoutedEventArgs e)
        {
            if (this.mainOriginal == null)
            {
                this.DisplayMessage("Nera pagrindinio paveiksliuko");

                return;
            }

            if (this.fragmentOriginal == null)
            {
                this.DisplayMessage("Nera fragmento paveiksliuko");

                return;
            }

            int acc;
            if (!int.TryParse(this.CompareColorSumAccuracy.Text, out acc) || acc < 1 || acc > 100)
            {
                this.DisplayMessage("Accuracy must be number between 1 and 100");

                return;
            }

            this.DisplayMessage("Vyksta lyginimas");

            var mainImage = BitmapVisualObject.Factory.Create(this.mainOriginal);
            var fragment = BitmapVisualObject.Factory.Create(this.fragmentOriginal);

            //var filter = new ColorSumPositionFilter(new UnsafeColorSumPaletteCalculator(), new UnsafeColorSumPaletteCalculator(), acc);
            //var resultedRectangles = filter.Find(mainImage, fragment, null);

            var scanner = new FragmentGridScanner(new ColorSumSimilarityCheck(new UnsafeColorSumPaletteCalculator(), new UnsafeColorSumPaletteCalculator(), acc));
            var resultedRectangles = scanner.Scan(mainImage, fragment);

            var im = new Bitmap(this.mainOriginal);

            if (resultedRectangles != null && resultedRectangles.Length > 0)
            {
                this.DisplayMessage("Rasta {0} fragmentu", resultedRectangles.Length);

                Graphics.FromImage(im).DrawRectangles(new System.Drawing.Pen(System.Drawing.Color.Red, 1), resultedRectangles);
            }
            else
            {
                this.DisplayMessage("Fragmentu nerasta");
            }

            this.MainImage.Source = this.bitmapConverter.Convert(im);
        }

        private void MakePng_Click(object sender, RoutedEventArgs e)
        {
            if (this.fragmentOriginal == null)
            {
                this.DisplayMessage("Nera fragmento paveiksliuko");

                return;
            }

            int x;
            if (!int.TryParse(this.PngX.Text, out x) || x < 0)
            {
                this.DisplayMessage("X must be non negative number");

                return;
            }

            int y;
            if (!int.TryParse(this.PngY.Text, out y) || y < 0)
            {
                this.DisplayMessage("X must be non negative number");

                return;
            }

            this.DisplayMessage("Saugomas PNG");

            var im = new Bitmap(this.fragmentOriginal);

            var opacity = im.GetPixel(x, y);
            im.MakeTransparent(opacity);

            var fileName = string.Format("{0}.png", Guid.NewGuid());

            im.Save(string.Format("C:\\work\\Examples\\GameAutomation\\{0}", fileName), System.Drawing.Imaging.ImageFormat.Png);
            
            this.DisplayMessage("Isaugotas PNG; {0}", fileName);
        }

        private void DisplayMessage(string message, params object[] pars)
        {
            this.Messages.Content = string.Format(message, pars);
        }

        private void MainImageScroll_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            scrollMousePoint = e.GetPosition(this.MainImageScroll);

            hOff = this.MainImageScroll.HorizontalOffset;
            vOff = this.MainImageScroll.VerticalOffset;

            this.MainImageScroll.CaptureMouse();
        }

        private void MainImageScroll_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (this.MainImageScroll.IsMouseCaptured)
            {
                this.MainImageScroll.ScrollToHorizontalOffset(hOff + (scrollMousePoint.X - e.GetPosition(this.MainImageScroll).X));
                this.MainImageScroll.ScrollToVerticalOffset(vOff + (scrollMousePoint.Y - e.GetPosition(this.MainImageScroll).Y));
            }
        }

        private void MainImageScroll_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.MainImageScroll.ReleaseMouseCapture();
        }

        private void MainImageScroll_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            this.MainImageScroll.ScrollToVerticalOffset(this.MainImageScroll.VerticalOffset + e.Delta);
        }
    }
}
