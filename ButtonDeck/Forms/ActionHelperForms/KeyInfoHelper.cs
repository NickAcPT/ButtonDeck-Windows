using NickAc.Backend.Objects.Implementation.DeckActions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ButtonDeck.Forms.ActionHelperForms
{
    public partial class KeyInfoHelper : TemplateForm
    {
        public KeyPressAction ModifiableAction { get; set; }

        public KeyInfoHelper()
        {
            InitializeComponent();
        }

        List<Keys> modifierKeys;
        List<Keys> nonModifierKeys;

        private void KeyInfoHelper_KeyDown(object sender, KeyEventArgs e)
        {
            List<Keys> modifiers = new List<Keys>();
            List<Keys> nonModifiers = new List<Keys>();
            foreach (Keys r in Enum.GetValues(typeof(Keys))) {
                if (e.Modifiers != Keys.None && e.Modifiers.HasFlag(r))
                    modifiers.Add(r);
                else if (e.KeyCode == (r))
                    nonModifiers.Add(r);
            }
            nonModifierKeys = nonModifiers;
            modifierKeys = modifiers;
            UpdateTextBox();
        }

        private void UpdateTextBox()
        {
            if (modifierKeys != null && nonModifierKeys != null) {
                textBox1.Text = string.Join(" + ", "[" + string.Join(" + ", modifierKeys.Where(c => c != Keys.None).Select(c => c.ToString()).OrderBy(c => c)) + "]", string.Join(" + ", nonModifierKeys.Where(c => !(c == Keys.ShiftKey || c == Keys.ControlKey || c == Keys.Menu))));
            }
        }

        private void KeyInfoHelper_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (Keys r in Enum.GetValues(typeof(Keys))) {
                if (e.Modifiers.HasFlag(r))
                    modifierKeys.Remove(r);
                else
                    nonModifierKeys.Remove(r);
            }
            UpdateTextBox();
        }
    }
}
