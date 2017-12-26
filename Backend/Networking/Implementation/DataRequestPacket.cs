using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NickAc.Backend.Networking.Attributes;
using NickAc.Backend.Networking.IO;

namespace NickAc.Backend.Networking.Implementation
{

    [Architecture(PacketArchitecture.ClientToServer)]
    public class DataRequestPacket : INetworkPacket
    {
        public override long GetPacketNumber() => 2;
        
        public override object Clone()
        {
            return new DataRequestPacket();
        }

        public override void FromInputStream(DataInputStream reader)
        {

        }

        public override void ToOutputStream(DataOutputStream writer)
        {
        }
    }
}
