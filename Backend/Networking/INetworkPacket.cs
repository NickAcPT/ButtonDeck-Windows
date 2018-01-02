using NickAc.Backend.Networking.Attributes;
using NickAc.Backend.Networking.Implementation;
using NickAc.Backend.Networking.IO;
using NickAc.Backend.Networking.TcpLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NickAc.Backend.Networking
{
    public static class NetworkPacketExtensions
    {

        public static bool IsStillFunctioning(this ConnectionState con)
        {
            try {
                var rep = con.RemoteEndPoint;
                return true;
            } catch (Exception) {
                return false;
            }
        }

        public static bool TryHeartbeat(this ConnectionState con)
        {
            try {
                Debug.WriteLine("Trying to send heartbeat to client!");
                var result = con.SendPacket(new HeartbeatPacket());
                if (result) Debug.WriteLine("Heartbeat success!");
                return result;
            } catch (Exception) {
                Debug.WriteLine("Heartbeat failure!");
                return false;
            }

        }

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
                    Debug.WriteLine($"Sending packet[ID:{packet.GetPacketNumber()}] to client!");
                    binaryWriter.Write(packet.GetPacketNumber());
                    Debug.WriteLine($"Sending data to output stream of packet[ID:{packet.GetPacketNumber()}]!");
                    packet.ToOutputStream(binaryWriter);
                    Debug.WriteLine($"Sending data to output stream of packet[ID:{packet.GetPacketNumber()}] - Success!");
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
                    binaryReader.Flush();
                    System.Diagnostics.Debug.WriteLine($"Read packet with ID: {packetNumber}");
                    Debug.WriteLine("Getting new packet by id!");
                    packet = DeckServiceProvider.GetNewNetworkPacketById(packetNumber);
                    Debug.WriteLine("Getting new packet by id - Success!");
                    Debug.WriteLine($"Getting data from stream[ID:{packetNumber}]!");
                    packet.FromInputStream(binaryReader);
                    Debug.WriteLine($"Getting data from stream[ID:{packetNumber}] - Success!");
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
