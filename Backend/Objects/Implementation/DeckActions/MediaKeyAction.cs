using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NickAc.Backend.Objects.Implementation.DeckActions
{
    public class MediaKeyAction : AbstractDeckAction
    {
        public enum MediaKeys
        {
            [Description("Previous")]
            Back,
            [Description("Next")]
            Next,
            [Description("Play/Pause")]
            PlayPause,
            [Description("Stop")]
            Stop,
            [Description("Volume Off")]
            VolumeOff,
            [Description("Volume Down")]
            VolumeMinus,
            [Description("Volume Up")]
            VolumePlus
        }

        [ActionPropertyInclude]
        public MediaKeys Key { get; set; } = MediaKeys.PlayPause;

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

        public Keys GetKeyFromMediaKey(MediaKeys mediaKey)
        {
            switch (mediaKey) {
                case MediaKeys.Back:
                    return Keys.MediaPreviousTrack;
                case MediaKeys.Next:
                    return Keys.MediaPreviousTrack;
                case MediaKeys.PlayPause:
                    return Keys.MediaPlayPause;
                case MediaKeys.Stop:
                    return Keys.MediaPlayPause;
                case MediaKeys.VolumeOff:
                    return Keys.VolumeMute;
                case MediaKeys.VolumeMinus:
                    return Keys.VolumeDown;
                case MediaKeys.VolumePlus:
                    return Keys.VolumeUp;
            }
            return Keys.None;
        }

        public override void OnButtonDown(DeckDevice deckDevice)
        {

        }

        public override void OnButtonUp(DeckDevice deckDevice)
        {
            
        }
    }
}
