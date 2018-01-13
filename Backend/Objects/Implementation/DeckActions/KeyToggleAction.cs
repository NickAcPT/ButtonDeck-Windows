using NickAc.Backend.Utils.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NickAc.Backend.Objects.Implementation.DeckActions
{
    public class KeyToggleAction : KeyPressAction
    {
        bool isPressed;
        public override void OnButtonDown(DeckDevice deckDevice)
        {
        }

        public override void OnButtonUp(DeckDevice deckDevice)
        {
            isPressed ^= true;
            if (!isPressed) {
                NativeKeyHandler.PressKey(KeyInfoValue.ModifierKeys);
                NativeKeyHandler.PressKey(KeyInfoValue.Keys);
            } else {
                NativeKeyHandler.UnpressKey(KeyInfoValue.Keys);
                NativeKeyHandler.UnpressKey(KeyInfoValue.ModifierKeys);
            }
        }

        public override AbstractDeckAction CloneAction()
        {
            return new KeyToggleAction();
        }

        public override string GetActionName()
        {
            return "Toggle Keypress";
        }

    }
}

