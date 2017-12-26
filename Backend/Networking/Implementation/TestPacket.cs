using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NickAc.Backend.Networking.Attributes;
using NickAc.Backend.Networking.IO;

namespace NickAc.Backend.Networking.Implementation
{
    [Architecture(PacketArchitecture.ServerToClient)]
    public class TestPacket : INetworkPacket
    {
        public override void FromInputStream(DataInputStream reader)
        {
        }

        public override long GetPacketNumber() => 3;

        public override void ToOutputStream(DataOutputStream writer)
        {
            writer.WriteBoolean(true);
            writer.WriteBoolean(false);
            writer.WriteInt(25);
            writer.WriteFloat(2.5f);
            writer.WriteUTF("NickAc");
        }
    }
}
