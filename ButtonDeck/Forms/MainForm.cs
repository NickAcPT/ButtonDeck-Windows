using NickAc.ModernUIDoneRight.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NickAc.ModernUIDoneRight.Objects;
using NickAc.ModernUIDoneRight.Controls;

namespace ButtonDeck.Forms
{
    public partial class MainForm : ModernForm
    {
        public MainForm()
        {
            InitializeComponent();
            ColorScheme = new ColorScheme(Color.FromArgb(45, 45, 45), Color.FromArgb(28, 28, 28));
            BackColor = Color.FromArgb(75, 75, 75);
            panel1.Controls.OfType<Control>().All((c) => {
                if (c is ModernButton mb) {
                    mb.Text = string.Empty;
                    mb.ColorScheme = ColorScheme;
                }
                return true;
            });
        }
    }
}
