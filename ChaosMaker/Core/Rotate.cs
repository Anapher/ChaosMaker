using System;
using System.Runtime.InteropServices;
using ChaosMaker.Native;

namespace ChaosMaker.Core
{
    class Rotate
    {
        // ReSharper disable InconsistentNaming
        public const int ENUM_CURRENT_SETTINGS = -1;
        public const int CDS_UPDATEREGISTRY = 0x1;
        public const int CDS_TEST = 0x2;

        public const int DM_DISPLAYORIENTATION = 0x80;
        public const int DM_PELSWIDTH = 0x80000;
        public const int DM_PELSHEIGHT = 0x100000;
        // ReSharper restore InconsistentNaming

        public RotateResult RotateScreen(RotateDegress pRotateDegrees, string deviceName = null)
        {
            var dm = new DEVMODE { dmDeviceName = new string((char)0, 32), dmFormName = new string((char)0, 32) };
            dm.dmSize = Convert.ToInt16(Marshal.SizeOf(dm));

            if ((UnsafeNativeMethods.EnumDisplaySettings(deviceName, ENUM_CURRENT_SETTINGS, ref dm) != 0))
            {
                // determine unrotated pixel sizes
                switch ((RotateDegress)dm.dmDisplayOrientation)
                {
                    case RotateDegress.Degrees90:
                    case RotateDegress.Degrees270:
                        SwapExtents(ref dm.dmPelsHeight, ref dm.dmPelsWidth);
                        break;
                }

                // set rotation parameters
                dm.dmDisplayOrientation = Convert.ToInt16(pRotateDegrees);
                switch (pRotateDegrees)
                {
                    case RotateDegress.Degrees90:
                    case RotateDegress.Degrees270:
                        SwapExtents(ref dm.dmPelsHeight, ref dm.dmPelsWidth);
                        break;
                }
                dm.dmFields = DM_DISPLAYORIENTATION | DM_PELSHEIGHT | DM_PELSWIDTH;

                // query change
                var rotRet = (RotateResult)UnsafeNativeMethods.ChangeDisplaySettings(ref dm, CDS_TEST);

                if (rotRet == RotateResult.RotateFailed)
                {
                    return RotateResult.RotateFailed;
                }
                //enact change
                rotRet = (RotateResult)UnsafeNativeMethods.ChangeDisplaySettings(ref dm, CDS_UPDATEREGISTRY);
                switch (rotRet)
                {
                    case RotateResult.RotateSuccessful:
                    case RotateResult.RotateRequiresRestart:
                        return rotRet;
                    default:
                        return RotateResult.RotateFailed;
                }
            }
            else
            {
                return RotateResult.RotateFailed;
            }

        }

        private static void SwapExtents(ref int pelHeight, ref int pelWidth)
        {
            pelHeight = pelHeight ^ pelWidth;
            pelWidth = pelWidth ^ pelHeight;
            pelHeight = pelHeight ^ pelWidth;
        }

    }

    public enum RotateDegress
    {
        Degrees0 = 0,
        Degrees90 = 1,
        Degrees180 = 2,
        Degrees270 = 3
    }


    public enum RotateResult
    {
        RotateFailed = -1,
        RotateSuccessful = 0,
        RotateRequiresRestart = 1
    }
}
