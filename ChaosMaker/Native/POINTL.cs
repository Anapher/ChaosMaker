using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace ChaosMaker.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct POINTL
    {
        public int x;
        public int y;
    }
}
