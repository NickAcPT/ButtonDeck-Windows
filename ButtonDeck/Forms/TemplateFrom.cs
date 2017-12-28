using ButtonDeck.Misc;
using NickAc.Backend.Utils;
using NickAc.ModernUIDoneRight.Forms;
using NickAc.ModernUIDoneRight.Objects;
using System;
using System.Collections.Generic;
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
        private ApplicationColorScheme _applicationColorScheme;

        private ApplicationColorScheme ApplicationColorScheme {
            get { return _applicationColorScheme; }
            set {
                _applicationColorScheme = value;
                ColorScheme = value;
                BackColor = value.BackgroundColor;
                Refresh();
            }
        }
        public TemplateForm()
        {
            if (ApplicationSettingsManager.Settings != null)
                LoadTheme(ApplicationSettingsManager.Settings.Theme);

            ColorSchemeCentral.ThemeChanged += (s, e) => {
                LoadTheme(ApplicationSettingsManager.Settings.Theme);
            };
        }

        protected override void OnLoad(EventArgs e)
        {
            if (ApplicationSettingsManager.Settings != null)
                LoadTheme(ApplicationSettingsManager.Settings.Theme);
            base.OnLoad(e);
            ModifyColorScheme(Controls.OfType<Control>());
        }

        private void ModifyColorScheme(IEnumerable<Control> cccc)
        {
            cccc.All(c => {
                ModifyColorScheme(c.Controls.OfType<Control>());
                try {
                    dynamic cc = c;
                    cc.ColorScheme = ColorScheme;
                    return true;
                } catch (Exception) {
                    return false;
                }
            });
        }

        private void LoadTheme(AppTheme theme)
        {
            switch (theme) {
                case AppTheme.Neptune:
                    ApplicationColorScheme = ColorSchemeCentral.Neptune;
                    break;
                case AppTheme.DarkSide:
                    ApplicationColorScheme = ColorSchemeCentral.DarkSide;
                    break;
                default:
                    break;
            }
        }
    }
}
