using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NickAc.Backend.Objects
{
    [Serializable]
    public class IDeckDevice
    {
        public IDeckDevice()
        {

        }
        public IDeckDevice(Guid deviceGuid, string deviceName)
        {
            DeviceGuid = deviceGuid;
            DeviceName = deviceName;
            MainFolder = new Implementation.DynamicDeckFolder();
        }

        public Guid DeviceGuid { get; set; }
        public String DeviceName { get; set; }
        public IDeckFolder MainFolder { get; set; }
    }
}
