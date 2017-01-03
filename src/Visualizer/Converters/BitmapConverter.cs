using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Visualizer.Converters
{
    public class BitmapConverter : IConverter<Bitmap, BitmapSource>, IConverter<BitmapSource, Bitmap>
    {
        public BitmapSource Convert(Bitmap source)
        {
            var bitmapData = source.LockBits(
                new Rectangle(0, 0, source.Width, source.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, source.PixelFormat);
            
            BitmapSource bitmapSource = null;

            try
            {
                var pf =
                    source.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb ? PixelFormats.Bgr24 : PixelFormats.Bgr32;

                bitmapSource = BitmapSource.Create(
                    bitmapData.Width, bitmapData.Height, 96, 96, pf, null,
                    bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);
            }
            finally
            {
                source.UnlockBits(bitmapData);
            }
            
            return bitmapSource;
        }

        /// <summary>
        /// BitmapSource to Bitmap converter using memory stream
        /// </summary>
        /// <param name="source"> BitmapSource argument from Image. </param>
        /// <returns> Bitmap. </returns>
        //public Bitmap Convert(BitmapSource source)
        //{
        //    var outStream = new MemoryStream();
        //    var enc = new BmpBitmapEncoder();

        //    enc.Frames.Add(BitmapFrame.Create(source));
        //    enc.Save(outStream);
        //    outStream.Seek(0, SeekOrigin.Begin);

        //    var bitmap = new Bitmap(outStream);

        //    return bitmap;
        //}

        public Bitmap Convert(BitmapSource source)
        {
            int width = source.PixelWidth;
            int height = source.PixelHeight;
            int stride = width * ((source.Format.BitsPerPixel + 7) / 8);
            IntPtr ptr = IntPtr.Zero;

            try
            {
                ptr = Marshal.AllocHGlobal(height * stride);
                source.CopyPixels(new Int32Rect(0, 0, width, height), ptr, height * stride, stride);

                using (var btm = new Bitmap(width, height, stride, System.Drawing.Imaging.PixelFormat.Format32bppRgb/*Format1bppIndexed*/, ptr))
                {
                    // Clone the bitmap so that we can dispose it and
                    // release the unmanaged memory at ptr
                    return new Bitmap(btm);
                }
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptr);
            }
        }
    }
}
