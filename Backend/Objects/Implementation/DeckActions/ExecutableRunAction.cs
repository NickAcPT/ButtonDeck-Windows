using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NickAc.Backend.Objects.Implementation.DeckActions
{
    public class ExecutableRunAction : AbstractDeckAction
    {
        [ActionPropertyInclude]
        [ActionPropertyDescription("To Execute")]
        public string ToExecute { get; set; } = "";

        public void ToExecuteHelper()
        {
            var originalToExec = new String(ToExecute.ToCharArray());
            dynamic form = Activator.CreateInstance(FindType("ButtonDeck.Forms.ActionHelperForms.ExecutableRunHelper")) as Form;
            var execAction = CloneAction() as ExecutableRunAction;
            execAction.ToExecute = ToExecute;
            form.ModifiableAction = execAction;

            if (form.ShowDialog() == DialogResult.OK) {
                ToExecute = form.ModifiableAction.ToExecute;
            } else {
                ToExecute = originalToExec;
            }
        }

        public override AbstractDeckAction CloneAction()
        {
            return new ExecutableRunAction();
        }

        public override DeckActionCategory GetActionCategory() => DeckActionCategory.General;

        public override string GetActionName() => "Run Executable";

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

        public override void OnButtonDown(DeckDevice deckDevice)
        {
            string exec = GetExecutable(ToExecute);
            var proc = new ProcessStartInfo(exec, ToExecute.Substring(exec.Length).Trim())
            {
                UseShellExecute = true,
            };
            Process.Start(proc);
        }

        public override void OnButtonUp(DeckDevice deckDevice)
        {
        }
    }
}
