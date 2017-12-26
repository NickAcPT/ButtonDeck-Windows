using NickAc.Backend.Networking.Attributes;
using NickAc.Backend.Networking.IO;
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
            if (!packet.CanClientReceive()) {
                throw new Exception($"Client can't receive NetworkPacket[ID: {packet.GetPacketNumber()}].");
            }
            byte[] bytesToSend = null;
            using (var memoryStream = new MemoryStream()) {
                using (var binaryWriter = new DataOutputStream(memoryStream)) {
                    binaryWriter.Write(packet.GetPacketNumber());
                    packet.ToOutputStream(binaryWriter);
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
                using (var binaryReader = new DataInputStream(memoryStream)) {
                    long packetNumber = binaryReader.ReadLong();
                    packet = DeckServiceProvider.GetNewNetworkPacketById(packetNumber);
                    packet.FromInputStream(binaryReader);
                    packet.Execute(con);
                }
            }
            if (!packet.CanServerReceive()) {
                throw new Exception($"Server can't receive NetworkPacket[ID: {packet.GetPacketNumber()}].");
            }
            return packet;
        }
    }

    public abstract class INetworkPacket : ICloneable
    {

        public abstract long GetPacketNumber();
        public abstract void FromInputStream(DataInputStream reader);
        public abstract void ToOutputStream(DataOutputStream writer);
        public virtual void Execute(ConnectionState state) { }

        public virtual object Clone()
        {
            throw new Exception($"NetworkPacket[ID: {GetPacketNumber()}] didn't implement Clone() method.");
        }
    }
}
