using NickAc.Backend.Networking.Attributes;
using NickAc.Backend.Networking.IO;
using NickAc.Backend.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NickAc.Backend.Networking.Implementation
{
    [Architecture(PacketArchitecture.ServerToClient)]
    public class SlotImageChangeChunkPacket : INetworkPacket
    {
        IDictionary<int, DeckImage> toSend = new Dictionary<int, DeckImage>();

        public void AddToQueue(int slot, DeckImage img)
        {
            toSend.Add(slot, img);
        }

        public override void FromInputStream(DataInputStream reader)
        {
        }

        public override long GetPacketNumber() => 7;

        public override void ToOutputStream(DataOutputStream writer)
        {
            //The number of images to send
            writer.WriteInt(toSend.Count);
            foreach (var item in toSend) {
                SendDeckImage(writer, item.Key, item.Value);
            }

        }

        private void SendDeckImage(DataOutputStream writer, int slot, DeckImage img)
        {
            if (img != null) {
                //Write the slot
                writer.WriteInt(slot);
                //Write the byte array lenght
                writer.WriteInt(img.InternalBitmap.Length);
                //Write the byte array
                writer.Write(img.InternalBitmap);
            }
        }

        public override object Clone()
        {
            return new SlotImageChangeChunkPacket();
        }
    }
}
