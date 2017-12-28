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

namespace ButtonDeck.Forms.FirstSetup
{
    public partial class ThemeSelectionPage : PageTemplate
    {
        public ThemeSelectionPage()
        {
            InitializeComponent();
            colorSchemePreviewControl2.AppTheme = ColorSchemeCentral.DarkSide;
            //TODO: Theme choosing
        }
    }
}
