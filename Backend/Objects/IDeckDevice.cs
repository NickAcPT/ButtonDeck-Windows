using NickAc.Backend.Networking.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NickAc.Backend.Objects
{
    [Serializable]
    public class DeckDevice
    {

        public class ButtonInteractionEventArgs : EventArgs
        {
            public ButtonInteractionEventArgs(int slotID, ButtonInteractPacket.ButtonAction performedAction)
            {
                SlotID = slotID;
                PerformedAction = performedAction;
            }

            public int SlotID { get; set; }
            public ButtonInteractPacket.ButtonAction PerformedAction { get; set; }
        }


        /// <summary>
        /// Called to signal to subscribers that a button was interacted with
        /// </summary>
        public event EventHandler<ButtonInteractionEventArgs> ButtonInteraction;
        public virtual void OnButtonInteraction(ButtonInteractPacket.ButtonAction performedAction, int slotID)
        {
            var eh = ButtonInteraction;

            eh?.Invoke(this, new ButtonInteractionEventArgs(slotID,
                performedAction));

        }

        public DeckDevice()
        {}

        public DeckDevice(Guid deviceGuid, string deviceName)
        {
            DeviceGuid = deviceGuid;
            DeviceName = deviceName;
            MainFolder = new Implementation.DynamicDeckFolder();
        }

        public Guid DeviceGuid { get; set; }
        public String DeviceName { get; set; }
        public IDeckFolder MainFolder { get; set; }
        [XmlIgnore]
        public IDeckFolder CurrentFolder { get; set; }

        public virtual void CheckCurrentFolder()
        {
            if (MainFolder != null && CurrentFolder == null)
                CurrentFolder = MainFolder;
        }
    }
}
