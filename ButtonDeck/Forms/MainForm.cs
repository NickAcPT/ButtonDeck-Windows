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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using static NickAc.Backend.Objects.AbstractDeckAction;

#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body

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
            panel1.Freeze();
            DevicesTitlebarButton item = new DevicesTitlebarButton(this);
            TitlebarButtons.Add(item);
            if (Program.Silent) {
                //Right now, we use the window redraw for device discovery purposes.
                //We need to simulate that with a timer.
                Timer t = new Timer
                {
                    //We should run it every 2 seconds and half.
                    Interval = 2500
                };
                t.Tick += (s, e) => {
                    //The discovery works by reading the Text from the button
                    item.RefreshCurrentDevices();
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
            ColorSchemeCentral.ThemeChanged += (s, e) =>
            ApplySidebarTheme(shadedPanel1);
        }

        public void ChangeButtonsVisibility(bool visible)
        {
            if (Disposing || !IsHandleCreated) return;
            Invoke(new Action(() => {
                Control control2 = Controls["label1"];
                if (control2 != null) control2.Visible = !visible;

                Control control = Controls["panel1"];
                if (control != null) control.Visible = visible;
                Control control3 = Controls["shadedPanel1"];
                if (control3 != null) control3.Visible = visible;
            }));
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
                new SettingsForm().ShowDialog();
            };
            appBar1.Actions.Add(item);

            AppAction itemTrash = new AppAction()
            {
                Image = imageTrash
            };
            itemTrash.Click += (s, ee) => {
                if (CurrentDevice == null) return;
                if (MessageBox.Show("Are you sure you  want to clear everything?" + Environment.NewLine + "All items will be lost!", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                    CurrentDevice.MainFolder = new DynamicDeckFolder();
                    SendItemsToDevice(CurrentDevice, true);
                    RefreshAllButtons(false);
                }
            };
            appBar1.Actions.Add(itemTrash);

            AppAction itemMagnetite = new AppAction();

            itemMagnetite.Click += (s, ee) => {
                new MagnetiteForm().ShowDialog();
            };
            appBar1.Actions.Add(itemMagnetite);

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
                        if (mb.Tag != null && mb.Tag is IDeckFolder folder && !(ee.Data.GetDataPresent(typeof(DeckItemMoveHelper)))) {
                            CurrentDevice.CurrentFolder = folder;
                            RefreshAllButtons(true);
                        } else if (mb.Tag != null && CurrentDevice.CurrentFolder != null && CurrentDevice.CurrentFolder.GetParent() != null && mb.CurrentSlot == 1 && mb.Tag is IDeckItem upItem) {
                            CurrentDevice.CurrentFolder = CurrentDevice.CurrentFolder.GetParent();
                            RefreshAllButtons(true);
                        }

                        if (ee.Data.GetDataPresent(typeof(DeckActionHelper)))
                            ee.Effect = DragDropEffects.Copy;
                        else if (ee.Data.GetDataPresent(typeof(DeckItemMoveHelper))) {
                            var item = ee.Data.GetData(typeof(DeckItemMoveHelper)) as DeckItemMoveHelper;
                            ee.Effect = item.CopyOld ? DragDropEffects.Copy : DragDropEffects.Move;
                        }
                    };
                    mb.DragDrop += (s, ee) => {
                        if (ee.Data.GetData(typeof(DeckActionHelper)) is DeckActionHelper action) {
                            if (ee.Effect == DragDropEffects.Copy) {
                                if (mb.Tag != null && mb.Tag is IDeckItem item) {
                                    if (CurrentDevice.CurrentFolder.GetParent() != null && mb.CurrentSlot == 1) return;
                                    if (item is IDeckFolder deckFolder) {
                                        var deckItemToAdd = new DynamicDeckItem
                                        {
                                            DeckAction = action.DeckAction.CloneAction()
                                        };
                                        var id2 = deckFolder.Add(deckItemToAdd);
                                        deckItemToAdd.DeckImage = new DeckImage(action.DeckAction.GetDefaultItemImage()?.Bitmap ?? Resources.img_item_default);

                                        CurrentDevice.CurrentFolder = deckFolder;
                                        RefreshAllButtons();

                                        FocusItem(GetButtonControl(id2), deckItemToAdd);

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
                                        DeckImage = new DeckImage(action.DeckAction.GetDefaultItemImage()?.Bitmap ?? Resources.img_item_default)
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
                                    mb.Image = ((DynamicDeckItem)mb.Tag).DeckAction.GetDefaultItemImage()?.Bitmap ?? Resources.img_item_default;

                                    FocusItem(mb, mb.Tag as IDeckItem);
                                }
                            }
                        } else if (ee.Data.GetDataPresent(typeof(DeckItemMoveHelper))) {
                            var action1 = ee.Data.GetData(typeof(DeckItemMoveHelper)) as DeckItemMoveHelper;
                            bool shouldMove = ee.Effect == DragDropEffects.Move;
                            if (shouldMove) {
                                action1.OldFolder.Remove(action1.OldSlot);
                                if (action1.OldFolder.GetParent() != null) {
                                    var oldItems = action1.OldFolder.GetDeckItems();
                                    bool isEmpty = action1.OldFolder.GetParent() != null ? oldItems.Count == 1 : oldItems.Count == 0;
                                    int slot = action1.OldFolder.GetParent().GetItemIndex(action1.OldFolder);
                                    if (isEmpty) {
                                        action1.OldFolder.GetParent().Remove(slot);
                                        RefreshButton(slot);
                                    }
                                }
                            }
                            IDeckItem item1 = shouldMove ? action1.DeckItem : action1.DeckItem.DeepClone();
                            if (item1 is IDeckFolder folder && !shouldMove) {
                                FixFolders(folder, false, CurrentDevice.CurrentFolder);
                            }
                            if (CurrentDevice.CurrentFolder.GetDeckItems().Any(cItem => CurrentDevice.CurrentFolder.GetItemIndex(cItem) == mb.CurrentSlot)) {
                                //We must create a folder if there is an item
                                var oldItem = action1.OldFolder.GetDeckItems().First(cItem => action1.OldFolder.GetItemIndex(cItem) == mb.CurrentSlot);

                                var newFolder = new DynamicDeckFolder
                                {
                                    DeckImage = new DeckImage(Resources.img_folder)
                                };
                                //Create a new folder instance
                                CurrentDevice.CheckCurrentFolder();
                                newFolder.ParentFolder = CurrentDevice.CurrentFolder;
                                newFolder.Add(1, folderUpItem);
                                newFolder.Add(oldItem);
                                newFolder.Add(action1.DeckItem);
                                CurrentDevice.CurrentFolder.Add(mb.CurrentSlot, newFolder);
                                CurrentDevice.CurrentFolder = newFolder;
                                RefreshAllButtons(true);
                            } else {
                                CurrentDevice.CurrentFolder.Add(mb.CurrentSlot, item1);
                                RefreshButton(action1.OldSlot, true);
                                RefreshButton(mb.CurrentSlot, true);
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

        public void RefreshAllButtons(bool sendToDevice = true)
        {
            Buttons_Unfocus(this, EventArgs.Empty);
            IDeckFolder folder = CurrentDevice?.CurrentFolder;
            for (int j = 0; j < 15; j++) {
                ImageModernButton control = GetButtonControl(j + 1);
                control.NormalImage = null;
                control.Tag = null;
                if (folder == null) control.Invoke(new Action(control.Refresh));
            }
            if (folder == null) return;
            for (int i = 0; i < folder.GetDeckItems().Count; i++) {
                IDeckItem item = null;
                item = folder.GetDeckItems()[i];
                ImageModernButton control = Controls.Find("modernButton" + folder.GetItemIndex(item), true).FirstOrDefault() as ImageModernButton;
                if (item != null) {
                    var ser = item.GetItemImage().BitmapSerialized;
                    control.NormalImage = item?.GetItemImage().Bitmap;
                    control.Tag = item;
                    control.Invoke(new Action(control.Refresh));
                }
            }
            CurrentDevice.CheckCurrentFolder();
            if (sendToDevice)
                SendItemsToDevice(CurrentDevice, folder);
        }

        public void RefreshButton(int slot, bool sendToDevice = true)
        {
            Buttons_Unfocus(this, EventArgs.Empty);

            IDeckFolder folder = CurrentDevice?.CurrentFolder;
            ImageModernButton control1 = GetButtonControl(slot);
            control1.NormalImage = null;
            control1.Tag = null;
            if (folder == null) control1.Invoke(new Action(control1.Refresh));

            if (folder == null) return;
            for (int i = 0; i < folder.GetDeckItems().Count; i++) {
                IDeckItem item = null;
                item = folder.GetDeckItems()[i];

                if (folder.GetItemIndex(item) != slot) continue;

                ImageModernButton control = Controls.Find("modernButton" + folder.GetItemIndex(item), true).FirstOrDefault() as ImageModernButton;
                if (item != null) {
                    var ser = item.GetItemImage().BitmapSerialized;
                    control.NormalImage = item?.GetItemImage().Bitmap;
                    control.Tag = item;
                    control.Invoke(new Action(control.Refresh));
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
            Invoke(new Action(() => {
                shadedPanel2.Hide();
                shadedPanel1.Refresh();
                Refresh();
            }));
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
                    ChangeToDevice(e.Device);
                }
                SendItemsToDevice(CurrentDevice, true);
            }));

            e.Device.ButtonInteraction += Device_ButtonInteraction;
        }

        public void ChangeToDevice(DeckDevice device)
        {
            CurrentDevice = device;
            LoadItems(CurrentDevice.CurrentFolder);
        }

        //List<Tuple<Guid, int>> ignoreOnce = new List<Tuple<Guid, int>>();
        private void Device_ButtonInteraction(object sender, DeckDevice.ButtonInteractionEventArgs e)
        {
            if (sender is DeckDevice device) {
                /*if (ignoreOnce.Any(c => c.Item1 == device.DeviceGuid && c.Item2 == e.SlotID)) {
                    ignoreOnce.Remove(ignoreOnce.First(c => c.Item1 == device.DeviceGuid && c.Item2 == e.SlotID));
                    return;
                }*/
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
                                    break;
                            }
                        }
                    } else if (item is DynamicDeckFolder deckFolder && e.PerformedAction == ButtonInteractPacket.ButtonAction.ButtonUp) {
                        device.CurrentFolder = deckFolder;
                        //ignoreOnce.Add(new Tuple<Guid, int>(device.DeviceGuid, e.SlotID));
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
                    var image = item.GetItemImage() ?? item.GetDefaultImage() ?? (new DeckImage(isFolder ? Resources.img_folder : Resources.img_item_default));
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
                var image = item.GetItemImage() ?? item.GetDefaultImage() ?? (new DeckImage(isFolder ? Resources.img_folder : Resources.img_item_default));
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

        private static DeckImage defaultDeckImage = new DeckImage(Resources.img_folder_up);
        private static DynamicDeckItem folderUpItem = new DynamicDeckItem() { DeckImage = defaultDeckImage };

        private void FixFolders(IDeckFolder folder, bool ignoreFirst = true, IDeckFolder trueParent = null)
        {
            if (!ignoreFirst) {
                if (trueParent != null)
                    folder.SetParent(trueParent);
                if (folder.GetParent() != null) {
                    folder.Add(1, folderUpItem);
                }
            }

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
            if (e.Device.DeviceGuid == CurrentDevice.DeviceGuid) {
                if (!DevicePersistManager.IsVirtualDeviceConnected) CurrentDevice = null;
                //Try to find a new device to be the current one.
                if (DevicePersistManager.PersistedDevices.Any(d => DevicePersistManager.IsDeviceConnected(d.DeviceGuid))) {
                    CurrentDevice = DevicePersistManager.PersistedDevices.First(d => DevicePersistManager.IsDeviceConnected(d.DeviceGuid));
                    if (IsHandleCreated && !Disposing)
                        Invoke(new Action(() => {
                            shadedPanel1.Show();
                            Buttons_Unfocus(sender, EventArgs.Empty);

                            e.Device.CheckCurrentFolder();
                            FixFolders(e.Device);

                            RefreshAllButtons(false);
                        }));
                }
            }
            if (IsHandleCreated && !Disposing)
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

        private Stopwatch lastClick = new Stopwatch();

        private void ItemButton_MouseClick(object sender, EventArgs e)
        {
            lastClick.Stop();
            bool isDoubleClick = lastClick.ElapsedMilliseconds != 0 && lastClick.ElapsedMilliseconds <= SystemInformation.DoubleClickTime;
            if (sender is ImageModernButton mb) {
                if (mb.Tag != null && mb.Tag is IDeckItem item) {
                    if (item is IDeckFolder folder) {
                        if (!isDoubleClick) {
                            FocusItem(mb, item);
                            goto end;
                        }
                        //Navigate to the folder
                        CurrentDevice.CurrentFolder = folder;
                        RefreshAllButtons();
                        goto end;
                    }
                    if (CurrentDevice.CurrentFolder.GetParent() != null) {
                        //Not on the main folder
                        if (mb.CurrentSlot == 1) {
                            CurrentDevice.CurrentFolder = CurrentDevice.CurrentFolder.GetParent();
                            RefreshAllButtons();
                            lastClick.Reset();
                            return;
                        }
                    }

                    //Show button panel with settable properties
                    FocusItem(mb, item);
                    lastClick.Reset();
                } else {
                    Buttons_Unfocus(sender, e);
                }
                return;
                end:
                lastClick.Reset();
                lastClick.Start();
            }
        }

        private void ItemButton_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            if (sender is ImageModernButton senderB) {
                if (!senderB.DisplayRectangle.Contains(e.Location)) return;
                if (e.Button == MouseButtons.Right && CurrentDevice.CurrentFolder.GetDeckItems().Any(c => CurrentDevice.CurrentFolder.GetItemIndex(c) == senderB.CurrentSlot)) {
                    var popupMenu = new ContextMenuStrip();

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

                    popupMenu.Items.Add("Clear image").Click += (s, ee) => {
                        if (senderB.Image != null && senderB.Image != Resources.img_folder && senderB.Image != Resources.img_item_default) {
                            senderB.Image.Dispose();
                            if (senderB != null && senderB.Tag != null && senderB.Tag is IDeckItem deckItem) {
                                bool isFolder = deckItem is IDeckFolder;
                                senderB.Image = isFolder ? Resources.img_folder : ((IDeckItem)senderB.Tag).GetDefaultImage()?.Bitmap ?? Resources.img_item_default;
                            }
                        }
                    };

                    popupMenu.Show(sender as Control, e.Location);
                }
                return;
            }
        }

        private void FocusItem(ImageModernButton mb, IDeckItem item)
        {
            flowLayoutPanel1.Controls.OfType<Control>().All(c => {
                c.Dispose();
                return true;
            });

            flowLayoutPanel1.Controls.Clear();
            if (item is DynamicDeckItem dI && dI.DeckAction != null) {
                label2.Text = dI.DeckAction.GetActionName();
                LoadProperties(dI, flowLayoutPanel1);
            } else if (item is IDeckFolder) {
                label2.Text = "Folder";
            }
            imageModernButton1.Origin = mb;
            imageModernButton1.Refresh();
            shadedPanel2.Show();
            shadedPanel1.Refresh();
        }

        private void LoadProperties(DynamicDeckItem item, FlowLayoutPanel panel)
        {
            var props = item.DeckAction.GetType().GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(ActionPropertyIncludeAttribute)));
            foreach (var prop in props) {
                bool shouldUpdateIcon = Attribute.IsDefined(prop, typeof(ActionPropertyUpdateImageOnChangedAttribute));
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
                    if (prop.PropertyType.IsSubclassOf(typeof(Enum))) {
                        var values = Enum.GetValues(prop.PropertyType);
                        panel.Controls.Add(new Label()
                        {
                            Text = GetPropertyDescription(prop)
                        });
                        ComboBox cBox = new ComboBox
                        {
                            DropDownStyle = ComboBoxStyle.DropDownList
                        };
                        cBox.Items.AddRange(values.OfType<Enum>().Select(c => EnumUtils.GetDescription(prop.PropertyType, c, c.ToString())).ToArray());

                        cBox.Text = EnumUtils.GetDescription(prop.PropertyType, (Enum)prop.GetValue(item.DeckAction), ((Enum)prop.GetValue(item.DeckAction)).ToString());

                        cBox.SelectedIndexChanged += (s, e) => {
                            try {
                                if (cBox.Text == string.Empty) return;
                                prop.SetValue(item.DeckAction, EnumUtils.FromDescription(prop.PropertyType, cBox.Text));
                                UpdateIcon(shouldUpdateIcon);
                            } catch (Exception) {
                                //Ignore all errors
                            }
                        };
                        panel.Controls.Add(cBox);
                        return;
                    }

                    if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom
                (typeof(string))) continue;
                    panel.Controls.Add(new Label()
                    {
                        Text = GetPropertyDescription(prop)
                    });

                    var txt = new TextBox
                    {
                        Text = (string)TypeDescriptor.GetConverter(prop.PropertyType).ConvertTo(prop.GetValue(item.DeckAction), typeof(string))
                    };
                    txt.TextChanged += (sender, e) => {
                        try {
                            if (txt.Text == string.Empty) return;
                            //After loosing focus, convert type to thingy.
                            prop.SetValue(item.DeckAction, TypeDescriptor.GetConverter(prop.PropertyType).ConvertFrom(txt.Text));
                            UpdateIcon(shouldUpdateIcon);
                        } catch (Exception) {
                            //Ignore all errors
                        }
                    };
                    txt.Width = panel.DisplayRectangle.Width - SystemInformation.VerticalScrollBarWidth * 2;
                    panel.Controls.Add(txt);
                }
            }

            ModifyColorScheme(flowLayoutPanel1.Controls.OfType<Control>());
        }

        private void UpdateIcon(bool shouldUpdateIcon)
        {
            if (shouldUpdateIcon) {
                imageModernButton1.Image = ((IDeckItem)imageModernButton1.Origin.Tag).GetDefaultImage()?.Bitmap ?? Resources.img_item_default;
                imageModernButton1.Refresh();
            }
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


        #region Events

        private bool mouseDown;
        private Point mouseDownLoc = Cursor.Position;

        private void ItemButton_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = e.Button == MouseButtons.Left;
            mouseDownLoc = Cursor.Position;
        }

        private void ItemButton_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDown) return;
            int distanceX = Math.Abs(mouseDownLoc.X - Cursor.Position.X);
            int distanceY = Math.Abs(mouseDownLoc.Y - Cursor.Position.Y);

            var finalPoint = new Point(distanceX, distanceY);
            bool didMove = SystemInformation.DragSize.Width * 2 > finalPoint.X && SystemInformation.DragSize.Height * 2 > finalPoint.Y;
            if (didMove && !finalPoint.IsEmpty) {
                mouseDown = false;
                if (sender is ImageModernButton mb) {
                    if (mb.Tag != null && mb.Tag is IDeckItem act) {
                        bool isDoubleClick = lastClick.ElapsedMilliseconds != 0 && lastClick.ElapsedMilliseconds <= SystemInformation.DoubleClickTime;
                        if (isDoubleClick) return;
                        if ((CurrentDevice.CurrentFolder.GetParent() != null && (mb.CurrentSlot == 1))) return;
                        mb.DoDragDrop(new DeckItemMoveHelper(act, CurrentDevice.CurrentFolder, mb.CurrentSlot) { CopyOld = ModifierKeys.HasFlag(Keys.Control) }, ModifierKeys.HasFlag(Keys.Control) ? DragDropEffects.Copy : DragDropEffects.Move);
                    }
                }
            }
        }
    }
    #endregion
}

#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body