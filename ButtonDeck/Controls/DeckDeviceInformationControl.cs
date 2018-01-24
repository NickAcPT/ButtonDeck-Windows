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
        private SizeF MeasureString(string text, Font font)
        {
            using (Bitmap bmp = new Bitmap(1,1)) {
                using (Graphics g = Graphics.FromImage(bmp)) {
                    return g.MeasureString(text, font);
                }
            }
        } 


        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            if (e is MouseEventArgs e2) {
                if (DeckDevice != null && NameLabelRectangle.Contains(e2.Location)) {
                    TextBox txtBox = new TextBox()
                    {
                        Bounds = NameLabelRectangleWithoutPrefix,
                        Width = Width - Padding.Right * 2,
                        Text = DeckDevice.DeviceName,
                        BorderStyle = BorderStyle.None,
                        BackColor = BackColor,
                        ForeColor = ForeColor,
                    };
                    txtBox.LostFocus += (s, ee) => {
                        if (txtBox.Text.Trim() != string.Empty) {
                            DeckDevice.DeviceName = txtBox.Text.Trim();
                            Refresh();
                        }
                        Controls.Remove(txtBox);
                    };
                    txtBox.KeyUp += (s, ee) => {
                        if (ee.KeyCode != Keys.Enter) return;
                        if (txtBox.Text.Trim() != string.Empty) {
                            DeckDevice.DeviceName = txtBox.Text.Trim();
                            Refresh();
                        }
                        Controls.Remove(txtBox);
                    };
                    Controls.Add(txtBox);
                    txtBox.Focus();

                }
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Focus();
        }

        public bool Selected {
            get { return _selected; }
            set {
                _selected = value;
                Invalidate();
            }
        }
        const string OFFLINE_PREFIX = "[OFFLINE]";
        public DeckDevice DeckDevice {
            get { return _deckDevice; }
            set {
                _deckDevice = value;
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


        public Rectangle NameLabelRectangle {
            get {
                if (DeckDevice == null) return Rectangle.Empty;
                int textHeight = (int)TextRenderer.MeasureText("AaBbCc", Font).Height;
                return new Rectangle(Padding.Left, Padding.Top, (int)MeasureString((deviceNamePrefix + DeckDevice.DeviceName), Font).Width, textHeight);
            }
        }
        public Rectangle NameLabelRectangleWithoutPrefix {
            get {
                if (DeckDevice == null) return Rectangle.Empty;
                Rectangle rect = NameLabelRectangle;
                return Rectangle.FromLTRB((int)(rect.Left + MeasureString(deviceNamePrefix, Font).Width), rect.Top, rect.Right, rect.Bottom);
            }
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
