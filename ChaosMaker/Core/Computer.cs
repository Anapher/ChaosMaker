using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using ChaosMaker.Native;
using Microsoft.Win32;

namespace ChaosMaker.Core
{
    public static class Computer
    {
        // ReSharper disable InconsistentNaming
        const int WM_COMMAND = 0x111;
        const int MIN_ALL = 419;
        const int MIN_ALL_UNDO = 416;

        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;
        // ReSharper restore InconsistentNaming

        public static void MinimizeAllScreens()
        {
            var lHwnd = UnsafeNativeMethods.FindWindow("Shell_TrayWnd", null);
            UnsafeNativeMethods.SendMessage(lHwnd, WM_COMMAND, (IntPtr)MIN_ALL, IntPtr.Zero);
        }

        public static void RestoreAllScreens()
        {
            IntPtr lHwnd = UnsafeNativeMethods.FindWindow("Shell_TrayWnd", null);
            UnsafeNativeMethods.SendMessage(lHwnd, WM_COMMAND, (IntPtr)MIN_ALL_UNDO, IntPtr.Zero);
        }

        public static void SetDesktopWallpaper(Image newWallpaper, Style style)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), "wallpaper.bmp");
            newWallpaper.Save(tempPath, ImageFormat.Bmp);

            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            if (key == null) return;

            switch (style)
            {
                case Style.Tiled:
                    key.SetValue(@"WallpaperStyle", 1.ToString());
                    key.SetValue(@"TileWallpaper", 1.ToString());
                    break;
                case Style.Centered:
                    key.SetValue(@"WallpaperStyle", 1.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                    break;
                case Style.Stretched:
                    key.SetValue(@"WallpaperStyle", 2.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(style));
            }

            UnsafeNativeMethods.SystemParametersInfo(SPI_SETDESKWALLPAPER,
                0,
                tempPath,
                SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }

        public static void ToggleDesktopIcons()
        {
            var toggleDesktopCommand = new IntPtr(0x7402);
            var hWnd = UnsafeNativeMethods.GetWindow(UnsafeNativeMethods.FindWindow("Progman", "Program Manager"), GetWindow_Cmd.GW_CHILD);
            UnsafeNativeMethods.SendMessage(hWnd, WM_COMMAND, toggleDesktopCommand, IntPtr.Zero);
        }

        public static bool RotateScreen(RotateDegress degress)
        {
            var rotate = new Rotate();
            bool allOk = true;
            foreach (var screen in Screen.AllScreens)
            {
                if (rotate.RotateScreen(degress, screen.DeviceName) == RotateResult.RotateFailed) allOk = false;
            }
            return allOk;
        }

        public static void SetTaskbarVisibility(bool isVisible)
        {
            Taskbar.Visible = isVisible;
        }

        public enum Style
        {
            Tiled,
            Centered,
            Stretched
        }
    }
}
