using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NickAc.Backend.Utils.AppSettings;
using ButtonDeck.Misc;

namespace ButtonDeck.Controls
{
    public partial class ColorSchemePreviewControl : UserControl
    {
        public ColorSchemePreviewControl()
        {
            InitializeComponent();
            Text = "Window Title";
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        public void ModifyColorScheme(IEnumerable<Control> cccc)
        {
            cccc.All(c => {
                ModifyColorScheme(c.Controls.OfType<Control>());
                try {
                    dynamic cc = c;
                    cc.Tag = "noColor";
                    cc.ForeColor = AppTheme.ForeColorShaded;
                    cc.ColorScheme = AppTheme;
                    return true;
                } catch (Exception) {
                    return true;
                }
            });
            //Refresh();
        }
        private void FixMyChild(IEnumerable<Control> cccc)
        {
            cccc.All(c => {
                FixMyChild(c.Controls.OfType<Control>());
                c.Tag = this;
                return true;
            });
        }

        
        private void HandleClickEvent(IEnumerable<Control> cccc)
        {
            cccc.All(c => {
                HandleClickEvent(c.Controls.OfType<Control>());
                c.Click += (s, e) => InvokeOnClick(this, e);
                return true;
            });
        }

        public void Recenter(Control c, bool horizontal = true, bool vertical = true)
        {
            if (c == null) return;
            if (horizontal)
                c.Left = (c.Parent.ClientSize.Width - c.Width) / 2;
            if (vertical)
                c.Top = (c.Parent.ClientSize.Height - c.Height) / 2;
        }

        private ApplicationColorScheme _appTheme = ColorSchemeCentral.Neptune;
        private AppTheme _underlyingAppTheme = NickAc.Backend.Utils.AppSettings.AppTheme.Neptune;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AppTheme = _appTheme;
            HandleClickEvent(Controls.OfType<Control>());
            FixMyTheme();
        }

        private void SetVisibility(IEnumerable<Control> ctrls, bool visible)
        {
            ctrls.All(c => {
                SetVisibility(c.Controls.OfType<Control>(), visible);
                c.Visible = visible;
                return true; });
        }

        Image toRender;

        private void FixMyTheme()
        {
            toRender = null;
            SetVisibility(Controls.OfType<Control>(), true);
            Refresh();
            Bitmap b = new Bitmap(Width, Height);
            DrawToBitmap(b, new Rectangle(0, 0, b.Width, b.Height));
            SetVisibility(Controls.OfType<Control>(), false);
            toRender = b;
            Refresh();

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (toRender != null) {
                e.Graphics.DrawImage(toRender, Point.Empty);
            }
        }

        public string DescriptionText {
            get { return label1.Text; }
            set {
                label1.Text = value;
                Recenter(label1, vertical: false);
            }
        }

        public AppTheme UnderlyingAppTheme {
            get { return _underlyingAppTheme; }
            set { _underlyingAppTheme = value; }
        }

        public ApplicationColorScheme AppTheme {
            get { return _appTheme; }
            set {
                _appTheme = value;
                ModifyColorScheme(Controls.OfType<Control>());
                titlebarPanel.BackColor = value.PrimaryColor;
                BackColor = value.BackgroundColor;
                FixMyTheme();
            }
        }

    }
}
