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

namespace ButtonDeck.Forms.FirstSetup
{
    public partial class DeviceNamePage : PageTemplate
    {
        public override void SaveProgress()
        {
            ApplicationSettingsManager.Settings.DeviceName = textBox1.Text.Trim();
            ApplicationSettingsManager.Settings.FirstRun = false;
            ApplicationSettingsManager.SaveSettings();
        }

        public override bool CanProgress { get => !(string.IsNullOrEmpty(textBox1.Text.Trim()) && string.IsNullOrWhiteSpace(textBox1.Text.Trim())); set => base.CanProgress = value; }
        public DeviceNamePage()
        {
            InitializeComponent();
        }
    }
}
