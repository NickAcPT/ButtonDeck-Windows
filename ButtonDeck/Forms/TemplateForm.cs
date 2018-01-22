using ButtonDeck.Controls;
using ButtonDeck.Misc;
using ButtonDeck.Properties;
using NickAc.Backend.Utils;
using NickAc.ModernUIDoneRight.Forms;
using NickAc.ModernUIDoneRight.Objects;
using NickAc.ModernUIDoneRight.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NickAc.Backend.Utils.AppSettings;

namespace ButtonDeck.Forms
{
    public class TemplateForm : ModernForm
    {
        public bool MinimizeToTrayRightMinimize { get; set; }

        private ApplicationColorScheme _applicationColorScheme;

        private ApplicationColorScheme ApplicationColorScheme {
            get { return _applicationColorScheme; }
            set {
                _applicationColorScheme = value;
                ColorScheme = new ColorScheme(value.PrimaryColor, value.SecondaryColor);
                BackColor = value.BackgroundColor;
                Refresh();
            }
        }
        public TemplateForm()
        {
            if (ApplicationSettingsManager.Settings != null) {
                LoadTheme(ApplicationSettingsManager.Settings.Theme);
                ModifyColorScheme(Controls.OfType<Control>());
            }
            ColorSchemeCentral.ThemeChanged += (s, e) => {
                LoadTheme(ApplicationSettingsManager.Settings.Theme);
                ModifyColorScheme(Controls.OfType<Control>());
            };
            Icon = Resources.button_deck;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (ApplicationSettingsManager.Settings != null) {
                LoadTheme(ApplicationSettingsManager.Settings.Theme);
            }
            base.OnLoad(e);
            ModifyColorScheme(Controls.OfType<Control>());

        }

        private static Color GetReadableForeColor(Color c)
        {
            return (((c.R + c.B + c.G) / 3) > 128) ? Color.Black : Color.White;
        }

        public void ModifyColorScheme(IEnumerable<Control> cccc)
        {
            cccc.All(c => {
                if (c.Tag != null && c.Tag.ToString() == "noColor")
                    return true;
                ModifyColorScheme(c.Controls.OfType<Control>());
                try {
                    dynamic cc = c;

                    if (c is TextBox txtBx) {
                        txtBx.BorderStyle = BorderStyle.FixedSingle;
                        cc.BackColor = _applicationColorScheme.BackgroundColor;
                        c.ForeColor = GetReadableForeColor(_applicationColorScheme.BackgroundColor);
                    }

                    cc.ForeColor = _applicationColorScheme.ForeColorShaded;
                    cc.ColorScheme = ColorScheme;
                    return true;
                } catch (Exception) {
                    return true;
                }
            });/*
    cccc.OfType<ColorSchemePreviewControl>().All(c => {
    c.AppTheme = ColorSchemeCentral.FromAppTheme(c.UnderlyingAppTheme);
    return true; });*/
        }



        private void LoadTheme(AppTheme theme)
        {
            ApplicationColorScheme = ColorSchemeCentral.FromAppTheme(theme);
        }
    }
}
