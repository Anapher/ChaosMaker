﻿using System;
using System.Diagnostics;
using System.Text;
using ChaosMaker.Native;

namespace ChaosMaker.Core
{
    /// <summary>
    /// Helper class for hiding/showing the taskbar and startmenu
    /// </summary>
    public static class Taskbar
    {
        // ReSharper disable InconsistentNaming
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        private const string VistaStartMenuCaption = "Start";
        private static IntPtr vistaStartMenuWnd = IntPtr.Zero;
        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Show the taskbar.
        /// </summary>
        public static void Show()
        {
            SetVisibility(true);
        }

        /// <summary>
        /// Hide the taskbar.
        /// </summary>
        public static void Hide()
        {
            SetVisibility(false);
        }

        /// <summary>
        /// Sets the visibility of the taskbar.
        /// </summary>
        public static bool Visible
        {
            set { SetVisibility(value); }
        }

        /// <summary>
        /// Hide or show the Windows taskbar and startmenu.
        /// </summary>
        /// <param name="show">true to show, false to hide</param>
        private static void SetVisibility(bool show)
        {
            // get taskbar window
            IntPtr taskBarWnd = UnsafeNativeMethods.FindWindow("Shell_TrayWnd", null);

            // try it the WinXP way first...
            IntPtr startWnd = UnsafeNativeMethods.FindWindowEx(taskBarWnd, IntPtr.Zero, "Button", "Start");

            if (startWnd == IntPtr.Zero)
            {
                // try an alternate way, as mentioned on CodeProject by Earl Waylon Flinn
                startWnd = UnsafeNativeMethods.FindWindowEx(IntPtr.Zero, IntPtr.Zero, (IntPtr)0xC017, "Start");
            }

            if (startWnd == IntPtr.Zero)
            {
                // ok, let's try the Vista easy way...
                startWnd = UnsafeNativeMethods.FindWindow("Button", null);

                if (startWnd == IntPtr.Zero)
                {
                    // no chance, we need to to it the hard way...
                    startWnd = GetVistaStartMenuWnd(taskBarWnd);
                }
            }

            UnsafeNativeMethods.ShowWindow(taskBarWnd, show ? SW_SHOW : SW_HIDE);
            UnsafeNativeMethods.ShowWindow(startWnd, show ? SW_SHOW : SW_HIDE);
        }

        /// <summary>
        /// Returns the window handle of the Vista start menu orb.
        /// </summary>
        /// <param name="taskBarWnd">windo handle of taskbar</param>
        /// <returns>window handle of start menu</returns>
        private static IntPtr GetVistaStartMenuWnd(IntPtr taskBarWnd)
        {
            // get process that owns the taskbar window
            uint procId;
            UnsafeNativeMethods.GetWindowThreadProcessId(taskBarWnd, out procId);

            Process p = Process.GetProcessById((int) procId);

            // enumerate all threads of that process...
            foreach (ProcessThread t in p.Threads)
            {
                UnsafeNativeMethods.EnumThreadWindows(t.Id, MyEnumThreadWindowsProc, IntPtr.Zero);
            }

            return vistaStartMenuWnd;
        }

        /// <summary>
        /// Callback method that is called from 'EnumThreadWindows' in 'GetVistaStartMenuWnd'.
        /// </summary>
        /// <param name="hWnd">window handle</param>
        /// <param name="lParam">parameter</param>
        /// <returns>true to continue enumeration, false to stop it</returns>
        private static bool MyEnumThreadWindowsProc(IntPtr hWnd, IntPtr lParam)
        {
            StringBuilder buffer = new StringBuilder(256);
            if (UnsafeNativeMethods.GetWindowText(hWnd, buffer, buffer.Capacity) > 0)
            {
                if (buffer.ToString() == VistaStartMenuCaption)
                {
                    vistaStartMenuWnd = hWnd;
                    return false;
                }
            }
            return true;
        }
    }
}
