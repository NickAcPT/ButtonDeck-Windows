using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NickAc.Backend.Utils.Native
{
    class NativeKeyHandler
    {
#pragma warning disable IDE1006 // Naming Styles
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
#pragma warning restore IDE1006 // Naming Styles


        const int VK_LSHIFT = 0xA0;
        const int VK_RSHIFT = 0xA1;
        const int VK_SHIFT = 0x10;
        const int VK_LCONTROL = 0xA2;
        const int VK_RCONTROL = 0xA3;
        const int VK_LMENU = 0xA4;
        const int VK_RMENU = 0xA5;


        public static void ClickKey(Keys[] keys)
        {
            const int KEYEVENTF_EXTENDEDKEY = 0x1;
            const int KEYEVENTF_KEYUP = 0x2;
            foreach (var k in keys) {
                keybd_event(GetKey(k), 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
            }
            foreach (var k in keys) {
                keybd_event(GetKey(k), 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
            }
        }
        public static void PressKey(Keys[] keys)
        {
            const int KEYEVENTF_EXTENDEDKEY = 0x1;
            foreach (var k in keys) {
                keybd_event(GetKey(k), 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
            }
        }

        private static byte GetKey(Keys k)
        {
            switch (k) {
                case Keys.Shift:
                case Keys.ShiftKey:
                case Keys.LShiftKey:
                    return VK_LSHIFT;
                case Keys.RShiftKey:
                    return VK_RSHIFT;

                case Keys.Alt:
                case Keys.LMenu:
                case Keys.Menu:
                    return VK_LMENU;
                case Keys.RMenu:
                    return VK_RMENU;

                case Keys.Control:
                case Keys.ControlKey:
                case Keys.LControlKey:
                    return VK_LCONTROL;
                case Keys.RControlKey:
                    return VK_RCONTROL;
            }
            return (byte)k;
        }

        public static void UnpressKey(Keys[] keys)
        {
            const int KEYEVENTF_EXTENDEDKEY = 0x1;
            const int KEYEVENTF_KEYUP = 0x2;

            foreach (var k in keys) {
                keybd_event(GetKey(k), 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
            }
        }
    }
}
