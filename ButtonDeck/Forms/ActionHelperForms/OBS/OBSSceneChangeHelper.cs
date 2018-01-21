using NickAc.Backend.Objects.Implementation.DeckActions.OBS;
using NickAc.Backend.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ButtonDeck.Forms.ActionHelperForms.OBS
{
    public partial class OBSSceneChangeHelper : TemplateForm
    {
        public SwitchSceneAction ModifiableAction { get; set; }

        public OBSSceneChangeHelper()
        {
            InitializeComponent();
        }

        private void CloseWithResult(DialogResult result)
        {
            DialogResult = result;
            Close();
        }

        private void ModernButton2_Click(object sender, EventArgs e)
        {
            ModifiableAction.SceneName = comboBox1.Text;
            CloseWithResult(DialogResult.OK);
        }

        private void ModernButton3_Click(object sender, EventArgs e)
        {
            CloseWithResult(DialogResult.OK);
        }

        private void OBSSceneChangeHelper_Load(object sender, EventArgs e)
        {
            comboBox1.Text = ModifiableAction.SceneName;
            Thread th = new Thread(() => {
                if (!OBSUtils.IsConnected)
                    OBSUtils.ConnectToServer();
                if (OBSUtils.IsConnected) {
                    Thread th2 = new Thread(() => {
                        var scenes = OBSUtils.GetScenes();
                        comboBox1.Invoke(new Action(() => {
                            comboBox1.Items.AddRange(scenes.ToArray());
                        }));
                    });
                    th2.Start();
                }
            });
            th.Start();

        }
    }
}
