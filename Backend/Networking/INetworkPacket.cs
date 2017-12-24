using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NickAc.Backend.Networking
{
    public abstract class INetworkPacket : ICloneable
    {

        public abstract long GetPacketNumber();
        public abstract void FromStreamReader(BinaryReader reader);
        public abstract void ToStreamWriter(BinaryWriter writer);

        public object Clone()
        {
            throw new NotImplementedException($"NetworkPacket[ID: {GetPacketNumber()}] didn't implement Clone() method.");
        }
    }
}
