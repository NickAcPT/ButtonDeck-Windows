using NickAc.Backend.Networking.TcpLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NickAc.Backend.Networking
{
    public static class NetworkPacketExtensions
    {
        /// <summary>
        /// Sends Data to the remote host.
        /// </summary>
        public static bool WriteString(this ConnectionState con, string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            return con.Write(bytes, 0, bytes.Length);
        }
        /// <summary>
        /// Sends a packet to the remote host.
        /// </summary>
        public static bool SendPacket(this ConnectionState con, INetworkPacket packet)
        {
            byte[] bytesToSend = null;
            using (var memoryStream = new MemoryStream()) {
                using (var binaryWriter = new BinaryWriter(memoryStream)) {
                    binaryWriter.Write(packet.GetPacketNumber());
                    packet.ToStreamWriter(binaryWriter);
                }
                bytesToSend = memoryStream.ToArray();
            }
            return con.Write(bytesToSend, 0, bytesToSend.Length);
        }

        /// <summary>
        /// Sends a packet to the remote host.
        /// </summary>
        public static INetworkPacket ReadPacket(this ConnectionState con, byte[] allData)
        {
            INetworkPacket packet;
            using (var memoryStream = new MemoryStream(allData)) {
                using (var binaryReader = new BinaryReader(memoryStream)) {
                    long packetNumber = binaryReader.ReadInt64();
                    packet = DeckServiceProvider.GetNewNetworkPacketById(packetNumber);
                    packet.FromStreamReader(binaryReader);
                    packet.Execute(con);
                }
            }
            return packet;
        }
    }

    public abstract class INetworkPacket : ICloneable
    {

        public abstract long GetPacketNumber();
        public abstract void FromStreamReader(BinaryReader reader);
        public abstract void ToStreamWriter(BinaryWriter writer);
        public virtual void Execute(ConnectionState state) { }

        public object Clone()
        {
            throw new NotImplementedException($"NetworkPacket[ID: {GetPacketNumber()}] didn't implement Clone() method.");
        }
    }
}
