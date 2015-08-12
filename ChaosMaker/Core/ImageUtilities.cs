using System.Drawing;
using System.Windows.Forms;

namespace ChaosMaker.Core
{
    class ImageUtilities
    {
        public static Bitmap TakeScreenshot()
        {
            var bmpScreenCapture = new Bitmap(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
            using (var g = Graphics.FromImage(bmpScreenCapture))
            {
                g.CopyFromScreen(SystemInformation.VirtualScreen.X,
                                 SystemInformation.VirtualScreen.Y,
                                 0, 0,
                                 bmpScreenCapture.Size,
                                 CopyPixelOperation.SourceCopy);
            }
            return bmpScreenCapture;
        }

        public static Bitmap RotateScreenshotScreenByScreen(Bitmap image)
        {
            var newImage = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(newImage))
            {
                foreach (var screen in Screen.AllScreens)
                {
                    var bitmap = image.Clone(screen.Bounds, image.PixelFormat);
                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    g.DrawImage(bitmap, new Point(screen.WorkingArea.X, screen.WorkingArea.Y));
                    bitmap.Dispose();
                }
            }
            return newImage;
        }
    }
}
