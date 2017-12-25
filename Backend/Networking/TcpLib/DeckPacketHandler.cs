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
            List<byte> allData = new List<byte>();
            byte[] buffer = new byte[1024];
            while (state.AvailableData > 0) {
                state.Read(buffer, 0, 1024);
                allData.AddRange(buffer);
            }
            state.ReadPacket()
        }
    }
}
