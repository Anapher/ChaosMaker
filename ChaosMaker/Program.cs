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
            if (args.Contains("/undo"))
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

            //Look here for the ids: https://msdn.microsoft.com/en-us/goglobal/bb895996.aspx?f=255&MSPPError=-2147217396
            KeyboardLayout.SwitchTo(2060); //French_Belgian

            Console.WriteLine(Properties.Resources.DoneText);
            Console.WriteLine(" Made by Alkalinee (Vincent)");

            if (Console.ReadLine() == "undo")
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