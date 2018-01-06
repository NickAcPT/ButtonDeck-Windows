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
using NickAc.Backend.Objects.Implementation;

namespace ButtonDeck.Controls
{
    public class ImageModernButton : ModernButton
    {

        public int CurrentSlot {
            get {
                try {
                    return int.Parse(ExtractNumber(Name));
                } catch (Exception) {
                    return -1;
                }
            }
        }
        public ImageModernButton Origin { get; set; }


        public static Guid GetConnectionGuidFromDeckDevice(DeckDevice device)
        {
            var connections = Program.ServerThread.TcpServer?.Connections.OfType<ConnectionState>().Where(c => c.IsStillFunctioning());
            return DevicePersistManager.DeckDevicesFromConnection.Where(m => connections.Select(c => c.ConnectionGuid).Contains(m.Key)).FirstOrDefault(m => m.Value.DeviceGuid == device.DeviceGuid).Key;
        }



        public string ExtractNumber(string original)
        {
            return new string(original.Where(Char.IsDigit).ToArray());
        }

        private Image _image;

        public Image NormalImage {
            get => Origin?._image ?? _image; set {
                if (Origin != null) {
                    Origin._image = value;
                    return;
                }
                _image = value;
                Refresh();
            }
        }

        public new Image Image {
            get => Origin?.Image ?? _image;
            set {
                if (Origin != null) {
                    Origin.Image = value;
                    return;
                }
                _image = value;
                Refresh();

                if (Parent != null && Parent.Parent != null && Parent.Parent is MainForm frm) {
                    if (frm.CurrentDevice != null) {
                        int slot = int.Parse(ExtractNumber(Name));
                        var connections = Program.ServerThread.TcpServer?.Connections.OfType<ConnectionState>().Where(c => c.IsStillFunctioning());
                        var stateID = GetConnectionGuidFromDeckDevice(frm.CurrentDevice);
                        var state = connections.FirstOrDefault(m => m.ConnectionGuid == stateID);
                        if (state != null) {
                            Bitmap bmp = new Bitmap(value);
                            var deckImage = new DeckImage(bmp);
                            if (Tag is DynamicDeckItem itemTag) {
                                itemTag.DeckImage = deckImage;
                            }
                            state.SendPacket(new SingleSlotImageChangePacket(deckImage)
                            {
                                ImageSlot = slot
                            });
                        }
                        if (Tag is DynamicDeckItem item) {
                            var device = frm.CurrentDevice;
                            device.CheckCurrentFolder();
                            device.CurrentFolder.Add(slot, item);
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
