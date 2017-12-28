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
        public TemplateForm()
        {
            if (ApplicationSettingsManager.Settings != null)
                LoadTheme(ApplicationSettingsManager.Settings.Theme);
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
                    ColorScheme = DefaultColorSchemes.Blue;
                    BackColor = Color.FromArgb(245, 245, 245);
                    break;
                case AppTheme.DarkSide:
                    ColorScheme = new ColorScheme(Color.FromArgb(45, 45, 45), Color.FromArgb(28, 28, 28));
                    BackColor = Color.FromArgb(75, 75, 75);
                    break;
                default:
                    break;
            }
        }
    }
}
