using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NickAc.Backend.Utils;
using Microsoft.Win32;

namespace ButtonDeck.Forms.FirstSetup
{
    public partial class DeviceNamePage : PageTemplate
    {
        private const string registryAppName = "ButtonDeckByNickAc";

        public static void AddApplicationToStartup()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)) {
                key.SetValue(registryAppName, "\"" + Application.ExecutablePath + "\" /s");
            }
        }

        public static void RemoveApplicationFromStartup()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)) {
                key.DeleteValue(registryAppName, false);
            }
        }

        public override void SaveProgress()
        {
            ApplicationSettingsManager.Settings.DeviceName = textBox1.Text.Trim();
            ApplicationSettingsManager.Settings.FirstRun = false;
            if (checkBox1.Checked) {
                AddApplicationToStartup();
            } else {
                RemoveApplicationFromStartup();
            }
            ApplicationSettingsManager.SaveSettings();
        }

        public override bool CanProgress { get => !(string.IsNullOrEmpty(textBox1.Text.Trim()) && string.IsNullOrWhiteSpace(textBox1.Text.Trim())); set => base.CanProgress = value; }
        public DeviceNamePage()
        {
            InitializeComponent();
        }
    }
}
