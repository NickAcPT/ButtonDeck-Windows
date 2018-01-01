using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NickAc.Backend.Objects.Implementation.DeckActions
{
    public class ExecutableRunAction : AbstractDeckAction
    {
        [ActionPropertyInclude]
        public string ToExecute { get; set; }

        public override DeckActionCategory GetActionCategory() => DeckActionCategory.General;

        public override string GetActionName() => "Run Executable";

        public override bool OnButtonClick(IDeckDevice deckDevice)
        {
            var proc = new ProcessStartInfo("cmd.exe", "/c " + ToExecute)
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Process.Start(proc);
            return true;
        }

        public override void OnButtonDown(IDeckDevice deckDevice)
        {
        }

        public override void OnButtonUp(IDeckDevice deckDevice)
        {
        }
    }
}
