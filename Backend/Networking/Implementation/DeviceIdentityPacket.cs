using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NickAc.Backend.Networking.Attributes;
using NickAc.Backend.Networking.IO;
using NickAc.Backend.Networking.TcpLib;
using NickAc.Backend.Objects;
using NickAc.Backend.Utils;

namespace NickAc.Backend.Networking.Implementation
{
    [Architecture(PacketArchitecture.ClientToServer | PacketArchitecture.ServerToClient)]
    public class DeviceIdentityPacket : INetworkPacket
    {
        public DeviceIdentityPacket()
        {

        }
        private readonly bool hasDeviceGuid;

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
            string receivedGuid = reader.ReadUTF().ToUpperInvariant();
            DeviceGuid = new Guid(receivedGuid);
            DeviceName = reader.ReadUTF();
        }

        public override void Execute(ConnectionState state)
        {
            IDeckDevice deckDevice = new IDeckDevice(DeviceGuid, DeviceName);

            DevicePersistManager.PersistDevice(deckDevice);
            DevicePersistManager.ChangeConnectedState(state, deckDevice);
        }


        public override void ToOutputStream(DataOutputStream writer)
        {
            //From server to client
            //Tell the client if they are going to receive a device Guid
            writer.WriteBoolean(!hasDeviceGuid);
            if (!hasDeviceGuid) {
                DeviceGuid = Guid.NewGuid();
                writer.WriteUTF(DeviceGuid.ToString());
            }
        }

        public override object Clone()
        {
            return new DeviceIdentityPacket();
        }
    }
}
