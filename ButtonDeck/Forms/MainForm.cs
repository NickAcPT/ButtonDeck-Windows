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
using ButtonDeck.Misc;
using System.IO;
using NickAc.Backend.Networking.IO;
using NickAc.Backend.Utils;
using ButtonDeck.Properties;
using NickAc.Backend.Objects;
using static NickAc.Backend.Objects.AbstractDeckAction;

namespace ButtonDeck.Forms
{
    public partial class MainForm : TemplateForm
    {
        public int ConnectedDevices { get => Program.ServerThread.TcpServer.CurrentConnections; }
        public MainForm()
        {
            InitializeComponent();

            TitlebarButtons.Add(new DevicesTitlebarButton(this));

            /*byte[] x;
            using (MemoryStream ms = new MemoryStream()) {
                using (DataOutputStream writer = new DataOutputStream(ms)) {
                    writer.WriteLong(2);
                    writer.WriteUTF("-");
                    writer.WriteUTF("Google Devicwiowfuhfwwiuf");
                    x = ms.ToArray();
                }
            }
            using (MemoryStream ms = new MemoryStream(x)) {
                using (DataInputStream reader = new DataInputStream(ms)) {
                    MessageBox.Show("" + reader.ReadLong());
                    MessageBox.Show("" + reader.ReadUTF());
                    MessageBox.Show("" + reader.ReadUTF());
                }
            }*/


        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var image = ColorScheme.ForegroundColor == Color.White ? Resources.ic_settings_white_48dp_2x : Resources.ic_settings_black_48dp_2x;
            AppAction item = new AppAction()
            {
                Image = Resources.ic_settings_white_48dp_2x
            };
            item.Click += (s, ee) => {
                //TODO: Settings
            };
            appBar1.Actions.Add(item);

            ApplyTheme(panel1);
            GenerateSidebar(shadedPanel1);
            ApplySidebarTheme(shadedPanel1);
            label1.ForeColor = ColorScheme.SecondaryColor;
        }

        private void GenerateSidebar(Control parent)
        {
            Padding categoryPadding = new Padding(5, 0, 0, 0);
            Font categoryFont = new Font(parent.Font.FontFamily, 13, FontStyle.Bold);
            Padding itemPadding = new Padding(25, 0, 0, 0);
            Font itemFont = new Font(parent.Font.FontFamily, 12);

            var items = ReflectiveEnumerator.GetEnumerableOfType<AbstractDeckAction>();
            List<Control> toAdd = new List<Control>();


            foreach (DeckActionCategory enumItem in Enum.GetValues(typeof(DeckActionCategory))) {
                var enumItems = items.Where(i => i.GetActionCategory() == enumItem);
                if (enumItems.Any()) {
                    toAdd.Add(new Label()
                    {
                        Padding = categoryPadding,
                        TextAlign = ContentAlignment.MiddleLeft,
                        Font = categoryFont,
                        Dock = DockStyle.Top,
                        Text = enumItem.ToString(),
                        Tag = "header",
                        Height = TextRenderer.MeasureText(enumItem.ToString(), categoryFont).Height
                    });
                    foreach (var i2 in enumItems) {
                        toAdd.Add(new Label()
                        {
                            Padding = itemPadding,
                            TextAlign = ContentAlignment.MiddleLeft,
                            Font = itemFont,
                            Dock = DockStyle.Top,
                            Text = i2.GetActionName(),
                            Height = TextRenderer.MeasureText(i2.GetActionName(), itemFont).Height,
                            Tag = i2,
                        });
                    }
                }

            }
            toAdd.AsEnumerable().Reverse().All(m => {
                parent.Controls.Add(m);
                return true;
            });

        }

        private void ApplyTheme(Control parent)
        {
            ApplicationColorScheme appTheme = ColorSchemeCentral.FromAppTheme(ApplicationSettingsManager.Settings.Theme);
            parent.Controls.OfType<Control>().All((c) => {
                if (c is ModernButton mb) {
                    mb.Text = string.Empty;
                    mb.ColorScheme = ColorScheme;
                }
                c.BackColor = appTheme.BackgroundColor;
                return true;
            });
        }

        private void ApplySidebarTheme(Control parent)
        {
            //Headers have the theme's secondary color as background
            //and the theme's foreground color as text color
            ApplicationColorScheme appTheme = ColorSchemeCentral.FromAppTheme(ApplicationSettingsManager.Settings.Theme);
            parent.Controls.OfType<Control>().All((c) => {
                if (c.Tag != null && c.Tag.ToString().ToLowerInvariant() == "header") {
                    c.BackColor = appTheme.SecondaryColor;
                    c.ForeColor = appTheme.ForegroundColor;
                } else {
                    c.BackColor = appTheme.BackgroundColor;
                }
                return true;
            });
        }

    }
}
