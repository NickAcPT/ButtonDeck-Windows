using NickAc.Backend.Utils.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NickAc.Backend.Objects.Implementation.DeckActions
{
    public class KeyPressAction : AbstractDeckAction
    {
        public class KeyInfo
        {
            public KeyInfo()
            {
            }
            public KeyInfo(Keys[] modifierKeys, Keys[] keys)
            {
                ModifierKeys = modifierKeys;
                Keys = keys;
            }

            public Keys[] ModifierKeys { get; set; } = new Keys[] { };
            public Keys[] Keys { get; set; } = new Keys[] { };
        }

        [ActionPropertyInclude]
        [ActionPropertyDescription("Keys")]
        public KeyInfo KeyInfoValue { get; set; } = new KeyInfo();

        public void KeyInfoValueHelper()
        {
            var keyInfo = new KeyInfo(KeyInfoValue.ModifierKeys, KeyInfoValue.Keys);
            dynamic form = Activator.CreateInstance(FindType("ButtonDeck.Forms.ActionHelperForms.KeyInfoHelper")) as Form;
            var execAction = CloneAction() as KeyPressAction;
            execAction.KeyInfoValue = KeyInfoValue;
            form.ModifiableAction = execAction;

            if (form.ShowDialog() == DialogResult.OK) {
                KeyInfoValue = form.ModifiableAction.KeyInfoValue;
            } else {
                KeyInfoValue = keyInfo;
            }
        }

        public override AbstractDeckAction CloneAction()
        {
            return new KeyPressAction();
        }

        public override DeckActionCategory GetActionCategory() => DeckActionCategory.General;

        public override string GetActionName()
        {
            return "Simulate Keypress";
        }

        public override void OnButtonDown(DeckDevice deckDevice)
        {
            NativeKeyHandler.PressKey(KeyInfoValue.ModifierKeys);
            NativeKeyHandler.PressKey(KeyInfoValue.Keys);
        }

        public override void OnButtonUp(DeckDevice deckDevice)
        {
            NativeKeyHandler.UnpressKey(KeyInfoValue.Keys);
            NativeKeyHandler.UnpressKey(KeyInfoValue.ModifierKeys);
        }
    }
}
