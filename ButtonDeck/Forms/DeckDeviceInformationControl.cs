using NickAc.Backend.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ButtonDeck.Forms
{
    class DeckDeviceInformationControl : Control
    {
        public IDeckDevice DeckDevice { get; set; }

        public new Padding Padding = new Padding(5);

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (DeckDevice == null) return;
            int textHeight = (int)e.Graphics.MeasureString("AaBbCc", Font).Height;
            using (var sb = new SolidBrush(ForeColor)) {
                e.Graphics.DrawString(DeckDevice.DeviceName, Font, sb, Padding.Left, Padding.Top);
                using (var sb2 = new SolidBrush(Color.FromArgb(150, ForeColor))) {
                    e.Graphics.DrawString("ID: " + DeckDevice.DeviceGuid, Font, sb, Padding.Left, Padding.Top + textHeight);
                }
            }
            
        }
    }
}
