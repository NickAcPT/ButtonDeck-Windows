using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NickAc.Backend.Networking.Implementation
{
    public class DataRequestPacket : INetworkPacket
    {
        

        public override long GetPacketNumber() => 2;

        public override void FromStreamReader(BinaryReader reader)
        {

        }

        public override void ToStreamWriter(BinaryWriter writer)
        {
            
        }

        public override object Clone()
        {
            return new DataRequestPacket();
        }
    }
}
