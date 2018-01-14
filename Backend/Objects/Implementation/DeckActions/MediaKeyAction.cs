using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NickAc.Backend.Objects.Implementation.DeckActions
{
    public class MediaKeyAction : AbstractDeckAction
    {
        public enum MediaKey
        {
            [Description("Previous")]
            Back,
            [Description("Next")]
            Next,
            [Description("Play/Pause")]
            PlayPause,
            [Description("Volume Off")]
            VolumeOff,
            [Description("Volume Down")]
            VolumeMinus,
            [Description("Volume Up")]
            VolumePlus
        }

        [ActionPropertyInclude]
        public MediaKey Key { get; set; } = MediaKey.PlayPause;

        public override AbstractDeckAction CloneAction()
        {
            return new MediaKeyAction();
        }

        public override DeckActionCategory GetActionCategory()
        {
            return DeckActionCategory.General;
        }

        public override string GetActionName()
        {
            return "Media Key Press";
        }

        [Obsolete]
        public override bool OnButtonClick(DeckDevice deckDevice)
        {
            throw new NotImplementedException();
        }

        public override void OnButtonDown(DeckDevice deckDevice)
        {
        }

        public override void OnButtonUp(DeckDevice deckDevice)
        {
            
        }
    }
}
