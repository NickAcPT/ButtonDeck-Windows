using NickAc.Backend.Networking;
using NickAc.Backend.Networking.TcpLib;
using NickAc.Backend.Objects;
using NickAc.Backend.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButtonDeck.Misc
{
    public static class IDeckDeviceExtensions
    {
        public static Guid GetConnectionGuidFromDeckDevice(IDeckDevice device)
        {
            var connections = Program.ServerThread.TcpServer?.Connections.OfType<ConnectionState>().Where(c => c.IsStillFunctioning());
            return DevicePersistManager.DeckDevicesFromConnection.Where(m => connections.Select(c => c.ConnectionGuid).Contains(m.Key)).FirstOrDefault(m => m.Value.DeviceGuid == device.DeviceGuid).Key;
        }
        public static ConnectionState GetConnection(this IDeckDevice device)
        {
            var connections = Program.ServerThread.TcpServer?.Connections.OfType<ConnectionState>().Where(c => c.IsStillFunctioning());
            var stateID = GetConnectionGuidFromDeckDevice(device);
            return connections.FirstOrDefault(m => m.ConnectionGuid == stateID);
        }

    }

}
