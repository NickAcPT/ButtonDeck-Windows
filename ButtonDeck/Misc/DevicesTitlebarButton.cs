using ButtonDeck.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ButtonDeck.Misc
{
    public class DevicesTitlebarButton : NickAc.ModernUIDoneRight.Objects.Interaction.ModernTitlebarButton
    {
        private readonly MainForm _frm;

        public DevicesTitlebarButton(MainForm frm)
        {
            _frm = frm;
        }

        public int CurrentConnections { get => Program.ServerThread.TcpServer.CurrentConnections; }
        public override string Text {
            get {
                Control control2 = _frm.Controls["label1"];
                if (control2 != null) control2.Visible = CurrentConnections == 0;

                Control control = _frm.Controls["panel1"];
                if (control != null) control.Visible = CurrentConnections > 0;
                return $"{CurrentConnections} Connected Device{(CurrentConnections != 1 ? "s" : "")}";
            }
            set => base.Text = value;
        }

        public override int Width { get => TextRenderer.MeasureText(Text, Font).Width + 8; set => base.Width = value; }

    }
}
