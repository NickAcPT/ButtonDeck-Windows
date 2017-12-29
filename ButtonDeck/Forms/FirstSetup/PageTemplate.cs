using ButtonDeck.Misc;
using NickAc.Backend.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ButtonDeck.Forms.FirstSetup
{
    public class PageTemplate : UserControl
    {
        public ApplicationColorScheme CurrentTheme {
            get {
                return ColorSchemeCentral.FromAppTheme(ApplicationSettingsManager.Settings.Theme);
            }
        }

        public override Color ForeColor {
            get => base.ForeColor; set {
                base.ForeColor = value;
                FixForeColor(Controls.OfType<Control>());
            }
        }

        private void FixForeColor(IEnumerable<Control> enumerable)
        {
            enumerable.All(c => {
                FixForeColor(c.Controls.OfType<Control>());
                c.ForeColor = ForeColor;

                return true;
            });
        }

        public PageTemplate()
        {
            MaximumSize = MinimumSize = Size = new Size(567, 244);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            BackColor = Color.Transparent;
            Font = new Font(SystemFonts.MessageBoxFont.FontFamily, 12);
            if (DesignMode)
                ForeColor = Color.White;
        }
    }
}
