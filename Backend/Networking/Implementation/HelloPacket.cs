using NickAc.Backend.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NickAc.Backend.Networking.Implementation
{
    public class HelloPacket : INetworkPacket
    {
        public int ProtocolVersion { get; set; }

        public override long GetPacketNumber() => 1;

        public override void FromStreamReader(BinaryReader reader)
        {
            ProtocolVersion = reader.ReadInt32();
        }

        public override void ToStreamWriter(BinaryWriter writer)
        {
            writer.Write(Constants.PROTOCOL_VERSION);
        }
    }
}
