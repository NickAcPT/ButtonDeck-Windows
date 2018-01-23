using ButtonDeck.Misc;
using NickAc.Backend.Objects;
using NickAc.Backend.Utils;
using NickAc.ModernUIDoneRight.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ButtonDeck.Forms
{
    class DeckDeviceInformationControl : Control
    {
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            lastClick.Stop();
            bool isDoubleClick = lastClick.ElapsedMilliseconds != 0 && lastClick.ElapsedMilliseconds <= SystemInformation.DoubleClickTime;


            return;
            end:
            lastClick.Reset();
            lastClick.Start();
        }

        public bool Selected {
            get { return _selected; }
            set { _selected = value;
                Invalidate();
            }
        }
        const string OFFLINE_PREFIX = "[OFFLINE]";
        public DeckDevice DeckDevice {
            get { return _deckDevice; }
            set { _deckDevice = value;
                deviceNamePrefix = (!DevicePersistManager.IsDeviceOnline(DeckDevice) ? $"{OFFLINE_PREFIX} " : "");
            }
        }

        public new Padding Padding = new Padding(5);
        private bool _selected;
        private DeckDevice _deckDevice;
        private string deviceNamePrefix;
        Stopwatch lastClick = new Stopwatch();



        private static Color GetReadableForeColor(Color c)
        {
            return (((c.R + c.B + c.G) / 3) > 128) ? Color.Black : Color.White;
        }

        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (DeckDevice == null) return;
            int textHeight = (int)e.Graphics.MeasureString("AaBbCc", Font).Height;
            var backgroundColor = Selected ? ColorSchemeCentral.FromAppTheme(ApplicationSettingsManager.Settings.Theme).SecondaryColor : Color.Transparent;
            using (var sb = new SolidBrush(Selected ? GetReadableForeColor(backgroundColor) : ForeColor)) {
                if (Selected) {
                    using (var sb2 = new SolidBrush(backgroundColor)) {
                        e.Graphics.FillRectangle(sb2, new Rectangle(Point.Empty, Size));
                    }
                }
                e.Graphics.DrawString(deviceNamePrefix + DeckDevice.DeviceName, Font, sb, Padding.Left, Padding.Top);
                using (var sb2 = new SolidBrush(Color.FromArgb(150, ForeColor))) {
                    e.Graphics.DrawString("ID: " + DeckDevice.DeviceGuid, Font, sb, Padding.Left, Padding.Top + textHeight);
                }
            }
            
        }
    }
}
