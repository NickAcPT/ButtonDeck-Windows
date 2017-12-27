using NickAc.Backend.Networking.Attributes;
using NickAc.Backend.Networking.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NickAc.Backend.Networking.Implementation
{
    [Architecture(PacketArchitecture.ServerToClient)]
    public class DesktopDisconnectPacket : INetworkPacket
    {
        public override object Clone()
        {
            return new DesktopDisconnectPacket();
        }
        public override void FromInputStream(DataInputStream reader)
        {
        }

        public override long GetPacketNumber() => 4;

        public override void ToOutputStream(DataOutputStream writer)
        {
        }
    }
}
