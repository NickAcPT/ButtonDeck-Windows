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
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags,
  UIntPtr dwExtraInfo);


        public void ClickKey(Keys[] keys)
        {
            const int KEYEVENTF_EXTENDEDKEY = 0x1;
            const int KEYEVENTF_KEYUP = 0x2;
            foreach (var k in keys) {
                keybd_event((byte)k, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
            }
            foreach (var k in keys) {
                keybd_event((byte)k, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
            }
        }
        public void PressKey(Keys[] keys)
        {
            const int KEYEVENTF_EXTENDEDKEY = 0x1;
            foreach (var k in keys) {
                keybd_event((byte)k, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
            }
        }
        public void UnpressKey(Keys[] keys)
        {
            const int KEYEVENTF_EXTENDEDKEY = 0x1;
            const int KEYEVENTF_KEYUP = 0x2;

            foreach (var k in keys) {
                keybd_event((byte)k, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
            }
        }
    }
}
