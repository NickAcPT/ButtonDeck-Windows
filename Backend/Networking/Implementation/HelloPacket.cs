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

        private bool HasDeviceGuid { get; set; }
        public Guid DeviceGuid { get; set; }

        public override long GetPacketNumber() => 1;


        public override void Execute(ConnectionState state)
        {
            if (ProtocolVersion != Constants.PROTOCOL_VERSION) {
                state.EndConnection();
            } else {
                state.SendPacket(new DeviceIdentityPacket(HasDeviceGuid));
            }
        }

        public override object Clone()
        {
            return new HelloPacket();
        }

        public override void FromInputStream(DataInputStream reader)
        {
            ProtocolVersion = reader.ReadInt();
            HasDeviceGuid = reader.ReadBoolean();
            if (HasDeviceGuid) {
                //We have a device GUID
                DeviceGuid = Guid.Parse(reader.ReadUTF());
            }
        }

        public override void ToOutputStream(DataOutputStream writer)
        {
        }
    }
}
