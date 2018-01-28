using NickAc.Backend.Properties;
using NickAc.Backend.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NickAc.Backend.Objects.Implementation.DeckActions.OBS
{
    public class StartStopStreamingAction : AbstractDeckAction
    {
        static bool firstTime;
        public enum StreamingState
        {
            Start,
            Stop,
            Toggle
        }
        [ActionPropertyInclude]
        [ActionPropertyDescription("Action")]
        [ActionPropertyUpdateImageOnChanged]
        public StreamingState StreamAction { get; set; }
        public override AbstractDeckAction CloneAction()
        {
            return new StartStopStreamingAction();
        }

        public override DeckActionCategory GetActionCategory()
        {
            return DeckActionCategory.OBS;
        }

        public override string GetActionName()
        {
            if (!firstTime) {
                firstTime ^= true;
                return "Start/Stop Streaming";
            }
            switch (StreamAction) {
                case StreamingState.Start:
                    return "Start Streaming";
                case StreamingState.Stop:
                    return "Stop Streaming";
            }
            return "Start/Stop Streaming";
        }

        public override DeckImage GetDefaultItemImage()
        {
            switch (StreamAction) {
                case StreamingState.Start:
                    return new DeckImage(Resources.img_item_start_stream);
                case StreamingState.Stop:
                    return new DeckImage(Resources.img_item_stop_stream);
                default:
                    return base.GetDefaultItemImage();
            }
        }

        public override void OnButtonDown(DeckDevice deckDevice)
        {
        }

        public override void OnButtonUp(DeckDevice deckDevice)
        {
            if (OBSUtils.IsConnected) {
                switch (StreamAction) {
                    case StreamingState.Start:
                        OBSUtils.StartStreaming();
                        break;
                    case StreamingState.Stop:
                        OBSUtils.StopStreaming();
                        break;
                }
            }
        }
    }
}
