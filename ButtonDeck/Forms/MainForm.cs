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

            panel1.Controls.OfType<Control>().All((c) => {
                if (c is ModernButton mb) {
                    mb.Text = string.Empty;
                    mb.ColorScheme = ColorScheme;
                }
                return true;
            });
            label1.ForeColor = ColorScheme.SecondaryColor;
        }
    }
}
