using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ButtonDeck.Misc;
using ButtonDeck.Controls;
using NickAc.Backend.Utils;
using NickAc.ModernUIDoneRight.Utils;

namespace ButtonDeck.Forms.FirstSetup
{
    public partial class ThemeSelectionPage : PageTemplate
    {
        public ThemeSelectionPage()
        {
            InitializeComponent();
            //Set the default theme
            colorSchemePreviewControl1.Tag = true;
            colorSchemePreviewControl2.AppTheme = ColorSchemeCentral.DarkSide;
            colorSchemePreviewControl2.UnderlyingAppTheme = AppSettings.AppTheme.DarkSide;
            //TODO: Theme choosing
        }
        


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            foreach (var c in Controls.OfType<ColorSchemePreviewControl>()) {
                if (c.Tag != null) {
                    using (var border = new SolidBrush(ControlPaint.DarkDark(CurrentTheme.SecondaryColor))) {
                        var rect = Rectangle.Inflate(c.Bounds, 2, 2);
                        e.Graphics.FillRectangle(border, rect);
                    }
                }
            }
        }

        private void ColorSchemePreviewControl2_Click(object sender, EventArgs e)
        {
            if (sender is ColorSchemePreviewControl ctrl) {
                Controls.OfType<ColorSchemePreviewControl>().All((c) => {
                    c.Tag = null;
                    return true;
                });
                ctrl.Tag = true;
                Refresh();
                ApplicationSettingsManager.Settings.Theme = ctrl.UnderlyingAppTheme;
                ColorSchemeCentral.OnThemeChanged(this);
            }
        }
    }
}
