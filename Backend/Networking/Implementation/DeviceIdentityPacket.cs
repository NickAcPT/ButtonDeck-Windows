using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NickAc.Backend.Networking.Attributes;
using NickAc.Backend.Networking.IO;

namespace NickAc.Backend.Networking.Implementation
{
    [Architecture(PacketArchitecture.ClientToServer | PacketArchitecture.ServerToClient)]
    public class DeviceIdentityPacket : INetworkPacket
    {
        public DeviceIdentityPacket()
        {

        }
        private bool hasDeviceGuid;

        public DeviceIdentityPacket(bool hasDeviceGuid)
        {
            this.hasDeviceGuid = hasDeviceGuid;
        }

        [ServerOnly]
        public Guid DeviceGuid { get; set; }

        [ClientOnly]
        public string DeviceName { get; set; }

        public override long GetPacketNumber() => 2;

        public override void FromInputStream(DataInputStream reader)
        {

        }

        public override void ToOutputStream(DataOutputStream writer)
        {
            //From server to client
            //Tell the client if they are going to receive a device Guid
            writer.WriteBoolean(true);
            //if (!hasDeviceGuid) {
            writer.WriteUTF(Guid.NewGuid().ToString());
            //}
        }
    }
}
