using NickAc.ModernUIDoneRight.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NickAc.Backend.Utils.AppSettings;

namespace ButtonDeck.Misc
{
    public static class ColorSchemeCentral
    {

        /// <summary>
        /// Called to signal to subscribers that the theme has been changed
        /// </summary>
        public static event EventHandler ThemeChanged;
        public static void OnThemeChanged(object e)
        {
            EventHandler eh = ThemeChanged;

            eh?.Invoke(e, EventArgs.Empty);
        }

        public static ApplicationColorScheme FromAppTheme(AppTheme th)
        {
            switch (th) {
                case AppTheme.Neptune:
                    return Neptune;
                case AppTheme.DarkSide:
                    return DarkSide;
                case AppTheme.KindaGreen:
                    return KindaGreen;
                default:
                    //Return Neptune as a fallback theme.
                    return Neptune;
            }
        }

        public static ApplicationColorScheme Neptune = new ApplicationColorScheme(DefaultColorSchemes.Blue, Color.FromArgb(245, 245, 245));

        public static ApplicationColorScheme KindaGreen = new ApplicationColorScheme(DefaultColorSchemes.Green, Color.FromArgb(245, 245, 245));

        public static ApplicationColorScheme DarkSide = new ApplicationColorScheme(new ColorScheme(Color.FromArgb(45, 45, 45), Color.FromArgb(28, 28, 28)), Color.FromArgb(75, 75, 75));
    }
}
