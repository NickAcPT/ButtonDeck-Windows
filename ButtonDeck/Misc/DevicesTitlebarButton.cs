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
using System.Threading;
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
            RefreshCurrentDevices();

            int controlSize = _frm.TitlebarHeight * 2;
            ModernForm frm = new ModernForm()
            {
                Sizable = false,
                ShowInTaskbar = false,
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

            //Load devices
            foreach (var device in DevicePersistManager.PersistedDevices) {
                try {

                    var ctrl = new DeckDeviceInformationControl()
                    {
                        DeckDevice = device,
                        Size = controlFinalSize,
                        ForeColor = _frm.ColorScheme.SecondaryColor,
                        Dock = DockStyle.Top,
                        Tag = _frm
                    };
                    frm.Controls.Add(ctrl);
                } catch (Exception) {
                    continue;
                }
            }

            frm.Show();
        }

        public void RefreshCurrentDevices()
        {
            Thread th = new Thread(UpdateConnectedDevices);
            th.Start();
        }

        public int CurrentConnections {
            get {
                return Program.ServerThread.TcpServer?.Connections.OfType<ConnectionState>().Select(m => m.ConnectionGuid).Count(DevicePersistManager.IsDeviceConnected) ?? 0;
            }
        }
        public override string Text {
            get {
                _frm.ChangeButtonsVisibility(DevicePersistManager.IsVirtualDeviceConnected || CurrentConnections > 0);
                
                return $"{CurrentConnections} Connected Device{(CurrentConnections != 1 ? "s" : "")}";
            }
            set => base.Text = value;
        }


        private void UpdateConnectedDevices()
        {
            List<Guid> toRemove = new List<Guid>();
            DevicePersistManager.DeckDevicesFromConnection.All(c => {
                if (!Program.ServerThread.TcpServer.Connections.OfType<ConnectionState>().Any(d => d.ConnectionGuid == c.Key)) {
                    toRemove.Add(c.Key);
                }
                return true;
            });
            toRemove.All(c => { DevicePersistManager.RemoveConnectionState(c); return true; });
        }

        public override int Width { get => TextRenderer.MeasureText(Text, Font).Width + 16; set => base.Width = value; }

    }
}
