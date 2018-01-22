using NickAc.Backend.Objects;
using NickAc.Backend.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ButtonDeck.Forms
{
    public partial class MagnetiteForm : TemplateForm
    {
        bool isDebugBuild;

        public MagnetiteForm()
        {
            InitializeComponent();
#if DEBUG
            isDebugBuild = true;
#endif
        }

        private void ModernButton4_Click(object sender, EventArgs e)
        {
            Program.ServerThread.Start();
        }

        private void ModernButton3_Click(object sender, EventArgs e)
        {
            Program.ServerThread.Stop();
            Program.ServerThread = new Misc.ServerThread();
        }

        private void ModernButton6_Click(object sender, EventArgs e)
        {
            OBSUtils.ConnectToOBS();
        }

        private void ModernButton5_Click(object sender, EventArgs e)
        {
            OBSUtils.Disconnect();
        }

        private void ModernButton7_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ButtonDeck Dump:");
            sb.AppendLine($"    - Protocol Version: {Constants.PROTOCOL_VERSION}");
            sb.AppendLine($"    - Port Number: {Constants.PORT_NUMBER}");
            sb.AppendLine($"    - Persisted Devices: {DevicePersistManager.PersistedDevices.Count}");
            sb.AppendLine($"    - Debug Build: {BooleanToString(isDebugBuild)}");
            sb.AppendLine($"    - OBS Nagged: {BooleanToString(ApplicationSettingsManager.Settings.OBSPluginNagged)}");
            sb.AppendLine($"    - Device Name: {ApplicationSettingsManager.Settings.DeviceName}");
            sb.AppendLine($"    - Persisted Devices: {DevicePersistManager.PersistedDevices.Count}");
            for (int i = 0, DevicePersistManagerPersistedDevicesCount = DevicePersistManager.PersistedDevices.Count; i < DevicePersistManagerPersistedDevicesCount; i++) {
                var device = DevicePersistManager.PersistedDevices[i];
                sb.AppendLine($"        - {i}: {device.DeviceName} [{device.DeviceGuid}]");
            }


            MessageBox.Show(sb.ToString());
        }

        private string BooleanToString(bool v)
        {
            return v ? "true" : "false";
        }
    }
}
