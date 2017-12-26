using NickAc.Backend.Networking.Attributes;
using NickAc.Backend.Networking.IO;
using NickAc.Backend.Networking.TcpLib;
using NickAc.Backend.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NickAc.Backend.Networking.Implementation
{
    [Architecture(PacketArchitecture.ClientToServer)]
    public class HelloPacket : INetworkPacket
    {
        public int ProtocolVersion { get; set; }

        public override long GetPacketNumber() => 1;


        public override void Execute(ConnectionState state)
        {
            if (ProtocolVersion != Constants.PROTOCOL_VERSION) {
                state.EndConnection();
            }
        }

        public override object Clone()
        {
            return new HelloPacket();
        }

        public override void FromInputStream(DataInputStream reader)
        {
            ProtocolVersion = reader.ReadInt();
        }

        public override void ToOutputStream(DataOutputStream writer)
        {
            writer.WriteInt(ProtocolVersion);
        }
    }
}
