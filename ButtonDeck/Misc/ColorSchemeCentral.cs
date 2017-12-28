using NickAc.ModernUIDoneRight.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButtonDeck.Misc
{
    public class ColorSchemeCentral
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

        public static ApplicationColorScheme Neptune = new ApplicationColorScheme(DefaultColorSchemes.Blue, Color.FromArgb(245, 245, 245));
        public static ApplicationColorScheme DarkSide = new ApplicationColorScheme(new ColorScheme(Color.FromArgb(45, 45, 45), Color.FromArgb(28, 28, 28)), Color.FromArgb(75, 75, 75));
    }
}
