using ButtonDeck.Forms;
using NickAc.Backend.Networking;
using NickAc.Backend.Networking.TcpLib;
using NickAc.Backend.Objects;
using NickAc.Backend.Utils;
using NickAc.ModernUIDoneRight.Controls;
using NickAc.ModernUIDoneRight.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NickAc.Backend.Networking.Implementation;

namespace ButtonDeck.Controls
{
    public class ImageModernButton : ModernButton
    {

        public static Guid GetConnectionGuidFromDeckDevice(IDeckDevice device)
        {
            var connections = Program.ServerThread.TcpServer?.Connections.OfType<ConnectionState>().Where(c => c.IsStillFunctioning());
            return DevicePersistManager.DeckDevicesFromConnection.Where(m=> connections.Select(c => c.ConnectionGuid).Contains(m.Key)).FirstOrDefault(m => m.Value.DeviceGuid == device.DeviceGuid).Key;
        }

        public string ExtractNumber(string original)
        {
            return new string(original.Where(Char.IsDigit).ToArray());
        }

        private Image _image;

        public new Image Image {
            get { return _image; }
            set {
                _image = value;
                if (Parent != null && Parent.Parent != null && Parent.Parent is MainForm frm) {
                    if (frm.CurrentDevice != null) {
                        var connections = Program.ServerThread.TcpServer?.Connections.OfType<ConnectionState>().Where(c => c.IsStillFunctioning());
                        var stateID = GetConnectionGuidFromDeckDevice(frm.CurrentDevice);
                        var state = connections.FirstOrDefault(m => m.ConnectionGuid == stateID);
                        if (state != null) {
                            Bitmap bmp = new Bitmap(value);
                            var deckImage = new DeckImage(bmp);
                            state.SendPacket(new SingleSlotImageChangePacket(deckImage)
                            {
                                ImageSlot = int.Parse(ExtractNumber(Name))
                            });
                        }
                    }
                }
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            if (Image != null) {
                pevent.Graphics.DrawImage(Image, DisplayRectangle);
            }
        }

    }
}
