using NickAc.Backend.Networking.TcpLib;
using NickAc.Backend.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NickAc.Backend.Utils
{
    public static class DevicePersistManager
    {
        private const string DEVICES_FILENAME = "devices.xml";
        private static IDictionary<Guid, IDeckDevice> deckDevicesFromConnection = new Dictionary<Guid, IDeckDevice>();

        public static ICollection<Guid> GuidsFromConnections {
            get {
                return deckDevicesFromConnection.Keys;
            }
        }

        private static List<IDeckDevice> persistedDevices;

        public static List<IDeckDevice> PersistedDevices {
            get {
                return persistedDevices;
            }
        }

        public static Guid GetConnectionGuidFromDeckDevice(IDeckDevice device)
        {
            return deckDevicesFromConnection.FirstOrDefault(m => m.Value.DeviceGuid == device.DeviceGuid).Key;
        }
        

        public static bool IsDeviceOnline(IDeckDevice device)
        {
            return deckDevicesFromConnection.Values.Any(m => m.DeviceGuid == device.DeviceGuid);
        }

        public static bool IsDevicePersisted(IDeckDevice device)
        {
            return IsDevicePersisted(device.DeviceGuid);
        }

        private static bool IsDevicePersisted(Guid deviceGuid)
        {
            return PersistedDevices.Any(w => w.DeviceGuid == deviceGuid);
        }

        public static void PersistDevice(IDeckDevice device)
        {
            if (IsDevicePersisted(device)) {
                PersistedDevices.RemoveAll(m => m.DeviceGuid == device.DeviceGuid);
            }
            PersistedDevices.Add(device);
        }

        public static void RemoveConnectionState(ConnectionState state)
        {
            deckDevicesFromConnection.Remove(state.ConnectionGuid);
        }

        public static void RemoveConnectionState(Guid state)
        {
            deckDevicesFromConnection.Remove(state);
        }

        public static void ChangeConnectedState(ConnectionState state, IDeckDevice device)
        {
            if (!state.Connected) {
                deckDevicesFromConnection.Remove(state.ConnectionGuid);
            } else {
                if (device == null) return;
                if (!deckDevicesFromConnection.ContainsKey(state.ConnectionGuid)) {
                    deckDevicesFromConnection.Add(state.ConnectionGuid, device);
                }
            }
        }

        public static void LoadDevices()
        {
            if (File.Exists(DEVICES_FILENAME)) {
                var newPersistedDevices = XMLUtils.FromXML<List<IDeckDevice>>(File.ReadAllText(DEVICES_FILENAME));
                if (persistedDevices == null) persistedDevices = new List<IDeckDevice>();
                persistedDevices.AddRange(newPersistedDevices.Where(m => !IsDevicePersisted(m.DeviceGuid)));
            } else {
                persistedDevices = new List<IDeckDevice>();
            }
        }

        public static void SaveDevices()
        {
            if (persistedDevices != null) {
                File.WriteAllText(DEVICES_FILENAME, XMLUtils.ToXML(persistedDevices));
            } else {
                File.Delete(DEVICES_FILENAME);
            }
        }
    }
}
