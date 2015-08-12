using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace ChaosMaker.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DEVMODE
    {

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmDeviceName;
        public short dmSpecVersion;
        public short dmDriverVersion;
        public short dmSize;
        public short dmDriverExtra;

        public int dmFields;
        // printer only
        //Public dmOrientation As Short
        //Public dmPaperSize As Short
        //Public dmPaperLength As Short
        //Public dmPaperWidth As Short
        //Public dmScale As Short
        //Public dmCopies As Short
        //Public dmDefaultSource As Short
        //Public dmPrintQuality As Short

        // display only fields
        public POINTL dmPosition;
        public int dmDisplayOrientation;

        public int DisplayFixedOutput;
        public short dmColor;
        public short dmDuplex;
        public short dmYResolution;
        public short dmTTOption;
        public short dmCollate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmFormName;
        public short dmLogPixels;
        public int dmBitsPerPel;
        public int dmPelsWidth;

        public int dmPelsHeight;
        public int dmDisplayFlags;

        public int dmDisplayFrequency;
        public int dmICMMethod;
        public int dmICMIntent;
        public int dmMediaType;
        public int dmDitherType;
        public int dmReserved1;

        public int dmReserved2;
        public int dmPanningWidth;

        public int dmPanningHeight;
    }

}
