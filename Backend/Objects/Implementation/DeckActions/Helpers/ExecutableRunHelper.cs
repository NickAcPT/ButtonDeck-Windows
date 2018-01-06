using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NickAc.Backend.Objects.Implementation.DeckActions.Helpers
{
    public partial class ExecutableRunHelper : Form
    {
        public ExecutableRunAction ModifiableAction { get; set; }
        public ExecutableRunHelper()
        {
            InitializeComponent();
        }
    }
}
