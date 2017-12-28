using NickAc.ModernUIDoneRight.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButtonDeck.Misc
{
    public class ApplicationColorScheme : ColorScheme
    {
        public ApplicationColorScheme(System.Drawing.Color primaryColor, System.Drawing.Color secondaryColor) : base(primaryColor, secondaryColor)
        {
        }


        public ApplicationColorScheme(ColorScheme baseC, Color backColor) : this(baseC.PrimaryColor, baseC.SecondaryColor)
        {
            BackgroundColor = backColor;
            ForeColorShaded = baseC.SecondaryColor;
        }
        public Color BackgroundColor { get; set; }
        public Color ForeColorShaded { get; set; }



    }
}
