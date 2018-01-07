using ButtonDeck.Controls;
using ButtonDeck.Misc;
using ButtonDeck.Properties;
using NickAc.Backend.Networking;
using NickAc.Backend.Networking.Implementation;
using NickAc.Backend.Objects;
using NickAc.Backend.Objects.Implementation;
using NickAc.Backend.Utils;
using NickAc.ModernUIDoneRight.Controls;
using NickAc.ModernUIDoneRight.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using static NickAc.Backend.Objects.AbstractDeckAction;

namespace ButtonDeck.Forms
{
    public partial class MainForm : TemplateForm
    {
        private static MainForm instance;

        public static MainForm Instance {
            get {
                return instance;
            }
        }

        private const int CLIENT_ARRAY_LENGHT = 1024 * 50;
        #region Constructors

        public MainForm()
        {
            instance = this;
            InitializeComponent();
            DevicesTitlebarButton item = new DevicesTitlebarButton(this);
            TitlebarButtons.Add(item);
            if (Program.Silent) {
                //Right now, we use the window redraw for device discovery purposes.
                //We need to simulate that with a timer.
                Timer t = new Timer
                {
                    //We should run it every half-second.
                    Interval = 500
                };
                t.Tick += (s, e) => {
                    //The discovery works by reading the Text from the button
                    var itemText = item.Text.Trim();
                };

                void handler(object s, EventArgs e)
                {
                    Hide();
                    Shown -= handler;

                }

                Shown += handler;
                t.Start();
                NotifyIcon icon = new NotifyIcon
                {
                    Icon = Icon,
                    Text = Text
                };
                icon.DoubleClick += (sender, e) => {
                    Show();
                };
                ContextMenuStrip menu = new ContextMenuStrip();
                menu.Items.Add("Open application").Click += (s, e) => {
                    Show();
                };
                menu.Items.Add("-");
                menu.Items.Add("Exit application").Click += (s, e) => {
                    Application.Exit();
                };
                FormClosing += (s, e) => {
                    if (e.CloseReason == CloseReason.UserClosing) {
                        Hide();
                        e.Cancel = true;
                    } else if (e.CloseReason == CloseReason.ApplicationExitCall) {
                        icon.Visible = false;
                        icon.Dispose();
                    }
                };
                menu.Opening += (s, e) => {
                    menu.Items[0].Select();
                };
                icon.ContextMenuStrip = menu;
                icon.Visible = true;
            }
        }

        ~MainForm()
        {
            instance = null;
        }

        #endregion

        #region Properties

        public int ConnectedDevices { get => Program.ServerThread.TcpServer.CurrentConnections; }
        public DeckDevice CurrentDevice { get; set; }

        #endregion

        #region Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DevicePersistManager.DeviceConnected += DevicePersistManager_DeviceConnected;

            DevicePersistManager.DeviceDisconnected += DevicePersistManager_DeviceDisconnected;
            var image = ColorScheme.ForegroundColor == Color.White ? Resources.ic_settings_white_48dp_2x : Resources.ic_settings_black_48dp_2x;
                var imageTrash = ColorScheme.ForegroundColor == Color.White ? Resources.ic_delete_white_48dp_2x : Resources.ic_delete_black_48dp_2x;
            AppAction item = new AppAction()
            {
                Image = image
            };
            item.Click += (s, ee) => {
                //TODO: Settings
            };
            //appBar1.Actions.Add(item);

            AppAction itemTrash = new AppAction()
            {
                Image = imageTrash
            };
            itemTrash.Click += (s, ee) => {
                if (MessageBox.Show("Are you sure you  want to clear everything?" + Environment.NewLine + "All items will be lost!", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                    CurrentDevice.MainFolder = new DynamicDeckFolder();
                    SendItemsToDevice(CurrentDevice, true);
                    RefreshAllButtons(false);
                }
            };
            appBar1.Actions.Add(itemTrash);

            ApplyTheme(panel1);
            GenerateSidebar(shadedPanel1);
            ApplySidebarTheme(shadedPanel1);
            shadedPanel2.Hide();
            shadedPanel1.Hide();
            Refresh();

            label1.ForeColor = ColorScheme.SecondaryColor;
        }

        private void ApplySidebarTheme(Control parent)
        {
            //Headers have the theme's secondary color as background
            //and the theme's foreground color as text color
            ApplicationColorScheme appTheme = ColorSchemeCentral.FromAppTheme(ApplicationSettingsManager.Settings.Theme);
            parent.Controls.OfType<Control>().All((c) => {
                if (c.Tag != null && c.Tag.ToString().ToLowerInvariant() == "header") {
                    c.BackColor = appTheme.SecondaryColor;
                    c.ForeColor = appTheme.ForegroundColor;
                } else {
                    c.BackColor = appTheme.BackgroundColor;
                }
                return true;
            });
        }

        private void ApplyTheme(Control parent)
        {
            ApplicationColorScheme appTheme = ColorSchemeCentral.FromAppTheme(ApplicationSettingsManager.Settings.Theme);
            parent.Controls.OfType<Control>().All((c) => {
                if (c is ImageModernButton mb) {
                    mb.AllowDrop = true;
                    mb.DragEnter += (s, ee) => {
                        if (ee.Data.GetDataPresent(typeof(DeckActionHelper)))
                            ee.Effect = DragDropEffects.Copy;
                    };
                    mb.DragDrop += (s, ee) => {
                        if (ee.Effect == DragDropEffects.Copy) {
                            if (ee.Data.GetData(typeof(DeckActionHelper)) is DeckActionHelper action) {
                                if (mb.Tag != null && mb.Tag is IDeckItem item) {
                                    if (item is IDeckFolder deckFolder) {

                                        mb.Tag = new DynamicDeckItem
                                        {
                                            DeckAction = action.DeckAction.CloneAction()
                                        };
                                        var id2 = deckFolder.Add(mb.Tag as IDeckItem);
                                        mb.Image = Resources.img_item_default;

                                        CurrentDevice.CurrentFolder = deckFolder;
                                        RefreshAllButtons();

                                        FocusItem(GetButtonControl(id2), mb.Tag as IDeckItem);

                                        return;
                                    }
                                    var folder = new DynamicDeckFolder
                                    {
                                        DeckImage = new DeckImage(Resources.img_folder)
                                    };
                                    //Create a new folder instance
                                    CurrentDevice.CheckCurrentFolder();
                                    folder.ParentFolder = CurrentDevice.CurrentFolder;
                                    folder.Add(1, folderUpItem);
                                    folder.Add(item);


                                    var newItem = new DynamicDeckItem
                                    {
                                        DeckAction = action.DeckAction.CloneAction(),
                                        DeckImage = new DeckImage(Resources.img_item_default)
                                    };

                                    var id = folder.Add(newItem);


                                    FocusItem(GetButtonControl(id), newItem);

                                    CurrentDevice.CurrentFolder.Add(mb.CurrentSlot, folder);

                                    mb.Tag = folder;
                                    mb.Image = Resources.img_folder;
                                    CurrentDevice.CurrentFolder = folder;
                                    RefreshAllButtons();

                                } else {
                                    mb.Tag = new DynamicDeckItem
                                    {
                                        DeckAction = action.DeckAction.CloneAction()
                                    };
                                    mb.Image = Resources.img_item_default;

                                    FocusItem(mb, mb.Tag as IDeckItem);
                                }
                            }
                        }
                    };
                    mb.Text = string.Empty;
                    mb.ColorScheme = ColorScheme;
                }
                c.BackColor = appTheme.BackgroundColor;
                return true;
            });
        }


        private void RefreshAllButtons( bool sendToDevice = true)
        {
            IDeckFolder folder = CurrentDevice.CurrentFolder;
            Bitmap empty = new Bitmap(1, 1);
            for (int j = 0; j < 15; j++) {
                ImageModernButton control = GetButtonControl(j + 1);
                control.NormalImage = null;
                control.Tag = null;
                control.Refresh();
            }

            for (int i = 0; i < folder.GetDeckItems().Count; i++) {
                IDeckItem item = null;
                item = folder.GetDeckItems()[i];
                ImageModernButton control = Controls.Find("modernButton" + folder.GetItemIndex(item), true).FirstOrDefault() as ImageModernButton;
                if (item != null) {
                    var ser = item.GetItemImage().BitmapSerialized;
                    control.NormalImage = item?.GetItemImage().Bitmap;
                    control.Tag = item;
                    control.Refresh();
                }

            }
            CurrentDevice.CheckCurrentFolder();
            if (sendToDevice)
                SendItemsToDevice(CurrentDevice, folder);
        }

        private ImageModernButton GetButtonControl(int id)
        {
            return Controls.Find("modernButton" + id, true).FirstOrDefault() as ImageModernButton;
        }

        private void Buttons_Unfocus(object sender, EventArgs e)
        {
            shadedPanel2.Hide();
        }

        private void DevicePersistManager_DeviceConnected(object sender, DevicePersistManager.DeviceEventArgs e)
        {
            Invoke(new Action(() => {
                shadedPanel1.Show();
                shadedPanel2.Hide();
                Refresh();

                e.Device.CheckCurrentFolder();
                FixFolders(e.Device);


                if (CurrentDevice == null) {
                    CurrentDevice = e.Device;
                    LoadItems(CurrentDevice.CurrentFolder);
                }
                SendItemsToDevice(CurrentDevice, true);
            }));

            e.Device.ButtonInteraction += Device_ButtonInteraction;
        }

        List<Tuple<Guid, int>> ignoreOnce = new List<Tuple<Guid, int>>();
        private void Device_ButtonInteraction(object sender, DeckDevice.ButtonInteractionEventArgs e)
        {
            if (sender is DeckDevice device) {
                if (ignoreOnce.Any(c=>c.Item1 == device.DeviceGuid && c.Item2 == e.SlotID)) {
                    ignoreOnce.Remove(ignoreOnce.First(c => c.Item1 == device.DeviceGuid && c.Item2 == e.SlotID));
                    return;
                }
                var currentItems = device.CurrentFolder.GetDeckItems();
                if (currentItems.Any(c => device.CurrentFolder.GetItemIndex(c) == e.SlotID + 1)) {
                    var item = currentItems.FirstOrDefault(c => device.CurrentFolder.GetItemIndex(c) == e.SlotID + 1);
                    if (item is DynamicDeckItem deckItem && !(item is IDeckFolder)) {
                        if (device.CurrentFolder.GetParent() != null) {
                            if (device.CurrentFolder.GetItemIndex(item) == 1) {
                                if (e.PerformedAction != ButtonInteractPacket.ButtonAction.ButtonUp) return;
                                //Navigate one up!
                                device.CurrentFolder = device.CurrentFolder.GetParent();
                                SendItemsToDevice(CurrentDevice, device.CurrentFolder);
                                RefreshAllButtons(false);
                                return;
                            }
                        }
                        if (deckItem.DeckAction != null) {
                            switch (e.PerformedAction) {
                                case ButtonInteractPacket.ButtonAction.ButtonDown:
                                    deckItem.DeckAction.OnButtonDown(device);
                                    break;
                                case ButtonInteractPacket.ButtonAction.ButtonUp:
                                    deckItem.DeckAction.OnButtonUp(device);
                                    deckItem.DeckAction.OnButtonClick(device);
                                    break;
                            }
                        }
                    } else if (item is DynamicDeckFolder deckFolder) {
                        device.CurrentFolder = deckFolder;
                        ignoreOnce.Add(new Tuple<Guid, int>(device.DeviceGuid, e.SlotID));
                        SendItemsToDevice(CurrentDevice, deckFolder);
                        RefreshAllButtons(false);
                    }
                }
            }
        }

        private static void SendItemsToDevice(DeckDevice device, bool destroyCurrent = false)
        {
            if (destroyCurrent) device.CurrentFolder = null;
            device.CheckCurrentFolder();
            SendItemsToDevice(device, device.CurrentFolder);
        }

        private static void SendItemsToDevice(DeckDevice device, IDeckFolder folder)
        {
            var con = device.GetConnection();
            if (con != null) {
                var packet = new SlotImageChangeChunkPacket();
                List<IDeckItem> items = folder.GetDeckItems();

                List<int> addedItems = new List<int>();

                for (int i = 0; i < 15; i++) {
                    IDeckItem item = null;
                    if (items.ElementAtOrDefault(i) != null) {
                        item = items[i];
                        addedItems.Add(folder.GetItemIndex(item));
                    }
                    if (item == null) continue;

                    bool isFolder = item is IDeckFolder;
                    var image = item.GetItemImage() ?? (new DeckImage(isFolder ? Resources.img_folder : Resources.img_item_default));
                    var seri = image.BitmapSerialized;

                    packet.AddToQueue(folder.GetItemIndex(item), image);
                }
                con.SendPacket(packet);

                var clearPacket = new SlotImageClearChunkPacket();
                for (int i = 1; i < 16; i++) {
                    if (addedItems.Contains(i)) continue;
                    clearPacket.AddToQueue(i);
                }

                con.SendPacket(clearPacket);

            }
        }

        private void LoadItems(IDeckFolder folder)
        {
            List<IDeckItem> items = folder.GetDeckItems();
            foreach (var item in items) {
                //This is when it loads.
                //It will load from the persisted device.

                bool isFolder = item is IDeckFolder;
                ImageModernButton control = Controls.Find("modernButton" + folder.GetItemIndex(item), true).FirstOrDefault() as ImageModernButton;
                var image = item.GetItemImage() ?? (new DeckImage(isFolder ? Resources.img_folder : Resources.img_item_default));
                var seri = image.BitmapSerialized;

                //TODO: ALLOW FOR SMALL BITMAP
                control.NormalImage = image.Bitmap;
                //control.Refresh();
                control.Tag = item;
            }
        }

        private void FixFolders(DeckDevice device)
        {
            FixFolders(device.MainFolder);
        }

        static DeckImage defaultDeckImage = new DeckImage(Resources.img_folder_up);
        static DynamicDeckItem folderUpItem = new DynamicDeckItem() { DeckImage = defaultDeckImage };
        private void FixFolders(IDeckFolder folder)
        {
            folder.GetSubFolders().All(c => {
                FixFolders(c);
                c.SetParent(folder);
                if (c.GetParent() != null) {
                    c.Add(1, folderUpItem);
                }

                return true;
            });
        }

        private void UnfixFolders(IDeckFolder folder)
        {
            folder.GetSubFolders().All(c => {
                UnfixFolders(c);
                c.SetParent(folder);
                if (c.GetParent() != null) {
                    c.Remove(1);
                }

                return true;
            });
        }

        private void DevicePersistManager_DeviceDisconnected(object sender, DevicePersistManager.DeviceEventArgs e)
        {
            Invoke(new Action(() => {
                shadedPanel2.Hide();
                shadedPanel1.Hide();
                Refresh();
            }));

            e.Device.ButtonInteraction -= Device_ButtonInteraction;
        }
        private void GenerateSidebar(Control parent)
        {
            Padding categoryPadding = new Padding(5, 0, 0, 0);
            Font categoryFont = new Font(parent.Font.FontFamily, 13, FontStyle.Bold);
            Padding itemPadding = new Padding(25, 0, 0, 0);
            Font itemFont = new Font(parent.Font.FontFamily, 12);

            var items = ReflectiveEnumerator.GetEnumerableOfType<AbstractDeckAction>();
            List<Control> toAdd = new List<Control>();

            foreach (DeckActionCategory enumItem in Enum.GetValues(typeof(DeckActionCategory))) {
                var enumItems = items.Where(i => i.GetActionCategory() == enumItem);
                if (enumItems.Any()) {
                    toAdd.Add(new Label()
                    {
                        Padding = categoryPadding,
                        TextAlign = ContentAlignment.MiddleLeft,
                        Font = categoryFont,
                        Dock = DockStyle.Top,
                        Text = enumItem.ToString(),
                        Tag = "header",
                        Height = TextRenderer.MeasureText(enumItem.ToString(), categoryFont).Height
                    });
                    foreach (var i2 in enumItems) {
                        Label item = new Label()
                        {
                            Padding = itemPadding,
                            TextAlign = ContentAlignment.MiddleLeft,
                            Font = itemFont,
                            Dock = DockStyle.Top,
                            Text = i2.GetActionName(),
                            Height = TextRenderer.MeasureText(i2.GetActionName(), itemFont).Height,
                            Tag = i2,
                        };

                        item.MouseDown += (s, ee) => {
                            if (item.Tag is AbstractDeckAction act)
                                item.DoDragDrop(new DeckActionHelper(act), DragDropEffects.Copy);
                        };
                        toAdd.Add(item);
                    }
                }
            }
            toAdd.AsEnumerable().Reverse().All(m => {
                parent.Controls.Add(m);
                return true;
            });
        }
        private void ImageModernButton1_MouseClick(object sender, MouseEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                Filter = ""
            };
            var herePath = Path.Combine(Directory.GetCurrentDirectory(), "Keys");
            if (Directory.Exists(herePath))
                dlg.InitialDirectory = herePath;

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            string sep = string.Empty;

            dlg.Filter = string.Format("Images ({0})|{0}|All files|*.*",
                string.Join(";", codecs.Select(codec => codec.FilenameExtension).ToArray()));


            dlg.DefaultExt = "png"; // Default file extension

            if (dlg.ShowDialog() == DialogResult.OK) {
                //We have an image file.
                //Load as bitmap and replace DeckImage
                try {
                    Bitmap bmp = new Bitmap(dlg.FileName);
                    if (DeckImage.ImageToByte(bmp).Length > CLIENT_ARRAY_LENGHT) {
                        MessageBox.Show(this, "The selected image is too big to be sent to the device. Please choose another", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ImageModernButton1_MouseClick(sender, new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0));
                        return;
                    }
                    imageModernButton1.Image = bmp;
                } catch (Exception) {
                }
            }
        }

        private void ItemButton_MouseClick(object sender, EventArgs e)
        {
            if (sender is ImageModernButton mb) {
                if (mb.Tag != null && mb.Tag is IDeckItem item) {
                    if (item is IDeckFolder folder) {
                        //Navigate to the folder
                        CurrentDevice.CurrentFolder = folder;
                        RefreshAllButtons();
                        return;
                    }
                    if (CurrentDevice.CurrentFolder.GetParent() != null) {
                        //Not on the main folder
                        if (mb.CurrentSlot == 1) {
                            CurrentDevice.CurrentFolder = CurrentDevice.CurrentFolder.GetParent();
                            RefreshAllButtons();
                        }
                    }
                    //Show button panel with settable properties

                    FocusItem(mb, item);


                } else {
                    Buttons_Unfocus(sender, e);
                }
            }
        }

        private void ItemButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (sender is ImageModernButton senderB) {
                if (!senderB.DisplayRectangle.Contains(e.Location)) return;
                if (e.Button == MouseButtons.Right) {
                    var popupMenu = new ContextMenuStrip();

                    popupMenu.Items.Add("Clear image").Click += (s, ee) => {
                        if (senderB.Image != null && senderB.Image != Resources.img_folder && senderB.Image != Resources.img_item_default) {
                            senderB.Image.Dispose();
                            if (senderB != null && senderB.Tag != null && senderB.Tag is IDeckItem deckItem) {
                                bool isFolder = deckItem is IDeckFolder;
                                senderB.Image = isFolder ? Resources.img_folder : Resources.img_item_default;
                            }
                        }
                    };

                    popupMenu.Items.Add("Remove item").Click += (s, ee) => {
                        if (senderB != null) {
                            if (senderB.Image != Resources.img_folder && senderB.Image != Resources.img_item_default) {
                                senderB.Image.Dispose();
                            }
                            senderB.Tag = null;
                            senderB.Image = null;
                            Buttons_Unfocus(sender, e);
                            CurrentDevice.CurrentFolder.Remove(senderB.CurrentSlot);

                        }
                    };

                    popupMenu.Show(sender as Control, e.Location);
                }
                return;
            }
        }

        private void FocusItem(ImageModernButton mb, IDeckItem item)
        {
            if (item is DynamicDeckItem dI && dI.DeckAction != null) {
                flowLayoutPanel1.Controls.OfType<Control>().All(c => {
                    c.Dispose();
                    return true;
                });

                flowLayoutPanel1.Controls.Clear();
                label2.Text = dI.DeckAction.GetActionName();
                LoadProperties(dI, flowLayoutPanel1);
                imageModernButton1.Origin = mb;
                imageModernButton1.Refresh();
                shadedPanel2.Show();
            }
        }

        private void LoadProperties(DynamicDeckItem item, FlowLayoutPanel panel)
        {
            var props = item.DeckAction.GetType().GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(ActionPropertyIncludeAttribute)) && TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom
                (typeof(string)));
            foreach (var prop in props) {
                MethodInfo helperMethod = item.DeckAction.GetType().GetMethod(prop.Name + "Helper");
                if (helperMethod != null) {
                    panel.Controls.Add(new Label()
                    {
                        Text = GetPropertyDescription(prop)
                    });

                    Button helperButton = new ModernButton()
                    {
                        Text = "..."
                    };

                    helperButton.Click += (sender, e) => helperMethod.Invoke(item.DeckAction, new object[] { });

                    helperButton.Width = panel.DisplayRectangle.Width - 16;
                    panel.Controls.Add(helperButton);

                } else {
                    panel.Controls.Add(new Label()
                    {
                        Text = GetPropertyDescription(prop)
                    });

                    var txt = new TextBox
                    {
                        Text = (string)TypeDescriptor.GetConverter(prop.PropertyType).ConvertTo(prop.GetValue(item.DeckAction), typeof(string))
                    };
                    txt.LostFocus += (sender, e) => {
                        try {
                            if (txt.Text == string.Empty) return;
                            //After loosing focus, convert type to thingy.
                            prop.SetValue(item.DeckAction, TypeDescriptor.GetConverter(prop.PropertyType).ConvertFrom(txt.Text));
                            txt.Text = string.Empty;
                        } catch (Exception ex) {
                            //Ignore all errors
                        }
                    };
                    txt.Width = panel.DisplayRectangle.Width - 16;
                    panel.Controls.Add(txt);
                }
            }

            ModifyColorScheme(flowLayoutPanel1.Controls.OfType<Control>());

        }

        private string GetPropertyDescription(PropertyInfo prop)
        {
            if (Attribute.IsDefined(prop, typeof(ActionPropertyDescriptionAttribute))) {
                var attrib = prop.GetCustomAttribute(typeof(ActionPropertyDescriptionAttribute)) as ActionPropertyDescriptionAttribute;
                return attrib.Description;
            }
            return prop.Name;
        }

        #endregion
    }
}