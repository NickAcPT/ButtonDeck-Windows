using NickAc.Backend.Networking;
using NickAc.Backend.Networking.TcpLib;
using NickAc.Backend.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static NickAc.Backend.Objects.DeckDevice;

namespace NickAc.Backend.Objects.Implementation.DeckActions.OBS
{
    public class TestSceneMultiSwitch : AbstractDeckAction
    {
        public override AbstractDeckAction CloneAction()
        {
            return new TestSceneMultiSwitch();
        }

        public override DeckActionCategory GetActionCategory()
        {
            return DeckActionCategory.OBS;
        }

        public override string GetActionName()
        {
            return "OBS Multi Scene Selector";
        }

        public override void OnButtonDown(DeckDevice deckDevice)
        {
        }

        private Font GetAdjustedFont(Graphics GraphicRef, string GraphicString, Font OriginalFont, int ContainerWidth, int MaxFontSize, int MinFontSize, bool SmallestOnFail)
        {
            Font testFont = null;
            // We utilize MeasureString which we get via a control instance           
            for (int AdjustedSize = MaxFontSize; AdjustedSize >= MinFontSize; AdjustedSize--) {
                testFont = new Font(OriginalFont.Name, AdjustedSize, OriginalFont.Style);

                // Test the string with the new size
                SizeF AdjustedSizeNew = GraphicRef.MeasureString(GraphicString, testFont);

                if (ContainerWidth > Convert.ToInt32(AdjustedSizeNew.Width)) {
                    // Good font, return it
                    return testFont;
                }
            }

            // If you get here there was no fontsize that worked
            // return MinimumSize or Original?
            if (SmallestOnFail) {
                return testFont;
            } else {
                return OriginalFont;
            }
        }

        public override void OnButtonUp(DeckDevice deckDevice)
        {
            Thread th = new Thread(() => {
                //We create a deck folder
                Size imageSize = new Size(512, 512);
                DynamicDeckFolder folder = new DynamicDeckFolder();
                Font defaultFont = new Font("Arial", 56, GraphicsUnit.Point);
                folder.SetParent(deckDevice.CurrentFolder);

                var scenes = OBSUtils.GetScenes();
                int index = 0;
                using (var bmp = new Bitmap(imageSize.Width, imageSize.Height)) {
                    using (var g = Graphics.FromImage(bmp)) {
                        g.DrawString("Exit", defaultFont, Brushes.Black, new RectangleF(Point.Empty, imageSize), new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
                    }
                    folder.Add(++index, new DynamicDeckItem() { DeckImage = new DeckImage(bmp) });
                }

                foreach (var s in scenes) {
                    using (var bmp = new Bitmap(imageSize.Width, imageSize.Height)) {
                        using (var g = Graphics.FromImage(bmp)) {
                            g.DrawString(s, defaultFont, Brushes.Black, new RectangleF(Point.Empty, imageSize), new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
                        }
                        folder.Add(++index, new DynamicDeckItem() { DeckImage = new DeckImage(bmp) });
                    }
                }

                try {
                    var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

                    Type callType = assemblies.SelectMany(a => a.GetTypes())
                                            .Single(t => t.FullName == "ButtonDeck.Misc.IDeckDeviceExtensions");

                    var method = callType.GetMethod("GetConnection", BindingFlags.Static | BindingFlags.Public);

                    ConnectionState connection = (ConnectionState)method.Invoke(null, new object[] { deckDevice });




                    deckDevice.CurrentFolder = folder;


                    //This is a local fuction. Don't be scared, because this can happen.
                    void fakeFolderHandle(object s, ButtonInteractionEventArgs e)
                    {
                        if (deckDevice.CurrentFolder != folder) return;
                        if (e.PerformedAction != Networking.Implementation.ButtonInteractPacket.ButtonAction.ButtonUp) return;
                        if (deckDevice.CurrentFolder.GetDeckItems().Any(c => deckDevice.CurrentFolder.GetItemIndex(c) == e.SlotID)) {
                            var item = deckDevice.CurrentFolder.GetDeckItems().Where(c => deckDevice.CurrentFolder.GetItemIndex(c) == e.SlotID);
                            if (item is IDeckItem && e.SlotID == 1) {
                                deckDevice.CurrentFolder = folder.ParentFolder;
                                SendFolder(connection, folder.ParentFolder);
                                deckDevice.ButtonInteraction -= fakeFolderHandle;
                                return;
                            }

                            if (scenes.AsEnumerable().ElementAtOrDefault(e.SlotID - 1) != null)
                                OBSUtils.SwitchScene(scenes[e.SlotID - 1]);
                        }

                    }

                    deckDevice.ButtonInteraction += fakeFolderHandle;

                    SendFolder(connection, folder);


                } catch (Exception e) {
                    //Don't trow. Just flow.
                }


            });
            th.Start();


        }

        private void SendFolder(ConnectionState connection, IDeckFolder folder)
        {
            List<int> list = new List<int>();

            var clearPacket = new Networking.Implementation.SlotImageClearChunkPacket();
            var packet = new Networking.Implementation.SlotImageChangeChunkPacket();
            foreach (var item in folder.GetDeckItems()) {
                if (!(item is DynamicDeckItem)) continue;
                var index = folder.GetItemIndex(item);
                list.Add(index);
                packet.AddToQueue(index, ((DynamicDeckItem)item).DeckImage);
            }
            for (int i = 0; i < 15; i++) {
                if (list.Contains(i + 1)) continue;
                clearPacket.AddToQueue(i + 1);
            }
            connection.SendPacket(packet);
            connection.SendPacket(clearPacket);
        }
    }
}
