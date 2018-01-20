using NickAc.Backend.Objects.Implementation.DeckActions;
using NickAc.Backend.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ButtonDeck.Forms.ActionHelperForms
{
    public partial class ExecutableRunHelper : TemplateForm
    {

        public static string GetExecutable(string command)
        {
            string executable = string.Empty;
            string[] tokens = command.Split(' ');

            for (int i = tokens.Length; i >= 0; i--) {
                executable = string.Join(" ", tokens, 0, i);
                if (File.Exists(executable.Trim('"')))
                    break;
            }
            return executable;
        }


        private string _toExecuteArguments;
        private string _toExecuteFileName;
        private ExecutableRunAction _modifiableAction;

        public ExecutableRunAction ModifiableAction {
            get { return _modifiableAction; }
            set {
                _modifiableAction = value;
                var exec = GetExecutable(value.ToExecute);
                textBox1.Text = exec.Trim();
                textBox2.Text = value.ToExecute.Substring(exec.Length).Trim();
            }
        }
        public ExecutableRunHelper()
        {
            InitializeComponent();
        }

        public string ToExecuteArguments {
            get { return _toExecuteArguments; }
            set {
                _toExecuteArguments = value;
                UpdateFinal(ModifiableAction);
            }
        }

        private void UpdateFinal(ExecutableRunAction act)
        {
            act.ToExecute = (_toExecuteFileName + " " + _toExecuteArguments).Trim();
        }

        public string ToExecuteFileName {
            get { return _toExecuteFileName; }
            set {
                _toExecuteFileName = value;
                UpdateFinal(ModifiableAction);
            }
        }

        private void ModernButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Executable Files (*.exe)|*.exe"
            };
            if (dlg.ShowDialog() == DialogResult.OK) {
                textBox1.Text = dlg.FileName;
            }

        }

        private void ModernButton2_Click(object sender, EventArgs e)
        {
            _toExecuteFileName = textBox1.Text;
            ToExecuteArguments = textBox2.Text;
            CloseWithResult(DialogResult.OK);
        }

        private void CloseWithResult(DialogResult result)
        {
            DialogResult = result;
            Close();
        }

        private void ModernButton3_Click(object sender, EventArgs e)
        {
            CloseWithResult(DialogResult.Cancel);
        }

        private void ExecutableRunHelper_Load(object sender, EventArgs e)
        {
            
        }
    }
}
