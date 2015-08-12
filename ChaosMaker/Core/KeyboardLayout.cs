using System;
using System.Windows.Forms;
using ChaosMaker.Native;

namespace ChaosMaker.Core
{
    public class KeyboardLayout
    {
        private static uint _attemptNo;

        public static ushort GetKeyboardLayout()
        {
            return
                UnsafeNativeMethods.GetKeyboardLayout(
                    (int)
                        UnsafeNativeMethods.GetWindowThreadProcessId(UnsafeNativeMethods.GetForegroundWindow(),
                            IntPtr.Zero));
        }

        public static long SetKeyboardLayout(string layoutToLoad) // "00000409" or "00000419"
        {
            long hklLayout = UnsafeNativeMethods.LoadKeyboardLayout(layoutToLoad, 1);  // 00000429
            UnsafeNativeMethods.PostMessage(UnsafeNativeMethods.GetForegroundWindow(), 0x0050, 2, 0);   // Layout changed. You think anyone checks TO WHAT?! just switch triggering
            return hklLayout;
        }

        public static void SwitchTo(uint layoutId) // 0x409 for ENG
        {
            if (GetKeyboardLayout() != layoutId)  // If not english ( 0x409 ) - switch
            {
                var hexString = layoutId.ToString("X");
                hexString = "00000000".Substring(hexString.Length) + hexString;
                SetKeyboardLayout(hexString);
                _attemptNo++;
            }
        }
    }
}