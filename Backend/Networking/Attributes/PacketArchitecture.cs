using System;
using System.Linq;

namespace NickAc.Backend.Networking.Attributes
{
    enum PacketArchitecture
    {
        ClientToServer = 2,
        ServerToClient = 4
    }

    public static class NetworkPacketExtensions
    {
        public static bool CanClientReceive(this INetworkPacket packet)
        {
            ArchitectureAttribute attrib = GetArchitectureFromPacket(packet);
            if (attrib == null)
                throw new Exception($"NetworkPacket[ID:{packet.GetPacketNumber()}] doesn't specify a packet architecture.");
            return attrib.Architecture.HasFlag(PacketArchitecture.ServerToClient);
        }

        public static bool CanServerReceive(this INetworkPacket packet)
        {
            ArchitectureAttribute attrib = GetArchitectureFromPacket(packet);
            if (attrib == null)
                throw new Exception($"NetworkPacket[ID:{packet.GetPacketNumber()}] doesn't specify a packet architecture.");
            return attrib.Architecture.HasFlag(PacketArchitecture.ClientToServer);
        }

        private static ArchitectureAttribute GetArchitectureFromPacket(INetworkPacket packet)
        {
            return packet
                .GetType()
                .GetCustomAttributes(typeof(ArchitectureAttribute), false)
                .OfType<ArchitectureAttribute>()
                .FirstOrDefault();
        }
    }
}
