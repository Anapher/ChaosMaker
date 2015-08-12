using System;
using System.Linq;
using System.Threading;
using ChaosMaker.Core;

namespace ChaosMaker
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Contains("/redo"))
            {
                Redo();
                return;
            }

            Computer.MinimizeAllScreens();
            Thread.Sleep(1000);
            using (var screenshot = ImageUtilities.TakeScreenshot())
            {
                var bitmap = ImageUtilities.RotateScreenshotScreenByScreen(screenshot);
                Computer.SetDesktopWallpaper(bitmap, Computer.Style.Tiled);
            }

            Computer.ToggleDesktopIcons();
            Computer.RotateScreen(RotateDegress.Degrees180);
            Computer.SetTaskbarVisibility(false);

            KeyboardLayout.SwitchTo(2060);

            Console.WriteLine(Properties.Resources.DoneText);
            Console.WriteLine(" Made by Alkalinee (Vincent)");

            if (Console.ReadLine() == "redo")
                Redo();
        }

        private static void Redo()
        {
            Computer.ToggleDesktopIcons();
            Computer.RotateScreen(RotateDegress.Degrees0);
            Computer.SetTaskbarVisibility(true);
            KeyboardLayout.SwitchTo(1031);
        }
    }
}