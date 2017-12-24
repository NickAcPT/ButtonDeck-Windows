using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NickAc.Backend.Networking.TcpLib
{
    public class DeckServiceProvider : NickAc.Backend.Networking.TcpLib.TcpServiceProvider
    {

        private static List<INetworkPacket> networkPackets = new List<INetworkPacket>();

        public static void RegisterNetworkPacket(INetworkPacket packet)
        {
            networkPackets.Add(packet);
        }

        public static INetworkPacket GetNewNetworkPacketById(long id)
        {
            try {
                return (NickAc.Backend.Networking.INetworkPacket)networkPackets.FirstOrDefault(p => p.GetPacketNumber() == id).Clone();
            } catch (Exception) {
                throw new NotImplementedException("NetworkPacket[ID: {id}] wasn't registered to the packet storage.");
            }
        }

        public override object Clone()
        {
            return new DeckServiceProvider();
        }

        public override void OnAcceptConnection(ConnectionState state)
        {

        }

        public override void OnDropConnection(ConnectionState state)
        {

        }

        public override void OnReceiveData(ConnectionState state)
        {
            using (var input = new MemoryStream(state._buffer)) {
                using (var reader = new BinaryReader(input)) {
                    var packetId = reader.ReadInt64();

                }
            }
        }
    }
}
