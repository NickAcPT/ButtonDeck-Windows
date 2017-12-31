using ButtonDeck.Forms;
using NickAc.Backend.Networking;
using NickAc.Backend.Networking.Implementation;
using NickAc.Backend.Networking.TcpLib;
using NickAc.Backend.Utils;
using NickAc.ModernUIDoneRight.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            Click += DevicesTitlebarButton_Click;
        }

        private void DevicesTitlebarButton_Click(object sender, MouseEventArgs e)
        {
            int controlSize = _frm.TitlebarHeight * 2;
            ModernForm frm = new ModernForm()
            {
                Sizable = false,
                BackColor = _frm.BackColor,
                ForeColor = _frm.ColorScheme.ForegroundColor,
                ColorScheme = _frm.ColorScheme,
                TitlebarVisible = false,
                MinimumSize = new Size(0, controlSize),
                Size = new Size(Width * 2, Math.Max(controlSize * CurrentConnections, controlSize) + 2)
            };
            //Hacky method to get this button rectangle on screen
            var rect = _frm.RectangleToScreen(_frm.TitlebarButtonsRectangle);
            rect.Width = rect.Width - (rect.Width - Width);
            frm.Location = new Point(rect.Right - frm.Width, rect.Bottom);
            frm.Deactivate += (s, ee) => frm.Dispose();
            Size controlFinalSize = new Size(frm.DisplayRectangle.Width, controlSize);

            if (CurrentConnections < 1) return;
            /*
            //Remove any unnecessary connections.
            List<Guid> toRemove = new List<Guid>();
            var connections = Program.ServerThread.TcpServer.Connections.OfType<ConnectionState>();
            toRemove.AddRange(DevicePersistManager.GuidsFromConnections.Where(g => connections.FirstOrDefault(c => c.ConnectionGuid == g) == null));
            toRemove.All(c => {DevicePersistManager.RemoveConnectionState(c); return true; });
            /*
            DevicePersistManager.GuidsFromConnections.Select(g => connections.FirstOrDefault(c => c.ConnectionGuid == g)).Where(c => c != null).Where(c => !c.TryHeartbeat()).All(c => {
                DevicePersistManager.RemoveConnectionState(c); return true;
            });*/
            //Load devices
            foreach (var device in DevicePersistManager.PersistedDevices.Where(DevicePersistManager.IsDeviceOnline)) {
                try {

                    var ctrl = new DeckDeviceInformationControl()
                    {
                        DeckDevice = device,
                        Size = controlFinalSize,
                        ForeColor = _frm.ColorScheme.SecondaryColor,
                        Dock = DockStyle.Top
                    };
                    frm.Controls.Add(ctrl);
                } catch (Exception) {
                    continue;
                }
            }

            frm.Show();



        }

        public int CurrentConnections {
            get {
                return Program.ServerThread.TcpServer?.Connections.OfType<ConnectionState>().Select(m => m.ConnectionGuid).Count(DevicePersistManager.IsDeviceConnected) ?? 0;
            }
        }
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

        public override int Width { get => TextRenderer.MeasureText(Text, Font).Width + 16; set => base.Width = value; }

    }
}
