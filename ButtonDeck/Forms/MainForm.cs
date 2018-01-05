using ButtonDeck.Controls;
using ButtonDeck.Misc;
using ButtonDeck.Properties;
using NickAc.Backend.Networking;
using NickAc.Backend.Networking.Implementation;
using NickAc.Backend.Objects;
using NickAc.Backend.Objects.Implementation;
using NickAc.Backend.Utils;
using NickAc.ModernUIDoneRight.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using static NickAc.Backend.Objects.AbstractDeckAction;

namespace ButtonDeck.Forms
{
    public partial class MainForm : TemplateForm
    {
        #region Constructors

        public MainForm()
        {
            InitializeComponent();

            TitlebarButtons.Add(new DevicesTitlebarButton(this));
        }

        #endregion

        #region Properties

        public int ConnectedDevices { get => Program.ServerThread.TcpServer.CurrentConnections; }
        public IDeckDevice CurrentDevice { get; set; }

        #endregion

        #region Methods

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            //Start saving stuff
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DevicePersistManager.DeviceConnected += DevicePersistManager_DeviceConnected;

            DevicePersistManager.DeviceDisconnected += DevicePersistManager_DeviceDisconnected;
            var image = ColorScheme.ForegroundColor == Color.White ? Resources.ic_settings_white_48dp_2x : Resources.ic_settings_black_48dp_2x;
            AppAction item = new AppAction()
            {
                Image = Resources.ic_settings_white_48dp_2x
            };
            item.Click += (s, ee) => {
                //TODO: Settings
            };
            appBar1.Actions.Add(item);

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
                                mb.Tag = new DynamicDeckItem
                                {
                                    DeckAction = action.DeckAction
                                };
                                mb.Image = Resources.img_item_default;
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
                if (CurrentDevice == null) {
                    CurrentDevice = e.Device;
                    List<IDeckItem> items = e.Device.MainFolder.GetDeckItems();
                    foreach (var item in items) {
                        //This is when it loads.
                        //It will load from the persisted device.

                        bool isFolder = item is IDeckFolder;
                        ImageModernButton control = Controls.Find("modernButton" + e.Device.MainFolder.GetItemIndex(item), true).FirstOrDefault() as ImageModernButton;
                        var image = item.GetItemImage() ?? (new DeckImage(isFolder ? Resources.img_folder : Resources.img_item_default));
                        var seri = image.BitmapSerialized;
                        control.NormalImage = image.Bitmap;
                        //control.Refresh();
                        if (item is DynamicDeckItem deckI)
                            control.Tag = deckI;
                    }
                }
                var con = e.Device.GetConnection();
                if (con != null) {
                    var packet = new SlotImageChangeChunkPacket();
                    List<IDeckItem> items = e.Device.MainFolder.GetDeckItems();
                    foreach (var item in items) {
                        bool isFolder = item is IDeckFolder;
                        var image = item.GetItemImage() ?? (new DeckImage(isFolder ? Resources.img_folder : Resources.img_item_default));
                        var seri = image.BitmapSerialized;

                        packet.AddToQueue(e.Device.MainFolder.GetItemIndex(item), image);
                    }
                    con.SendPacket(packet);
                }
            }));
        }

        private void DevicePersistManager_DeviceDisconnected(object sender, DevicePersistManager.DeviceEventArgs e)
        {
            Invoke(new Action(() => {
                shadedPanel2.Hide();
                shadedPanel1.Hide();
                Refresh();
            }));
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
        private void ImageModernButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                Filter = ""
            };

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            string sep = string.Empty;

            foreach (var c in codecs) {
                string codecName = c.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                dlg.Filter = String.Format("{0}{1}{2} ({3})|{3}", dlg.Filter, sep, codecName, c.FilenameExtension.ToLower());
                sep = "|";
            }

            dlg.Filter = String.Format("{0}{1}{2} ({3})|{3}", dlg.Filter, sep, "All Files", "*.*");

            dlg.DefaultExt = "png"; // Default file extension

            if (dlg.ShowDialog() == DialogResult.OK) {
                //We have an image file.
                //Load as bitmap and replace DeckImage
                try {
                    Bitmap bmp = new Bitmap(dlg.FileName);
                    imageModernButton1.Image = bmp;
                } catch (Exception) {
                }
            }
        }

        private void ItemButton_Click(object sender, EventArgs e)
        {
            if (sender is ImageModernButton mb) {
                if (mb.Tag != null && mb.Tag is IDeckItem item) {
                    if (item is IDeckFolder) {
                        //TODO: Require two clicks
                        return;
                    }
                    //Show button panel with settable properties
                    flowLayoutPanel1.Controls.OfType<Control>().All(c => {
                        c.Dispose();
                        return true;
                    });

                    flowLayoutPanel1.Controls.Clear();
                    if (item is DynamicDeckItem dI) {
                        label2.Text = dI.DeckAction.GetActionName();

                        LoadProperties(dI, flowLayoutPanel1);
                    }
                    imageModernButton1.Origin = mb;
                    shadedPanel2.Show();


                } else {
                    Buttons_Unfocus(sender, e);
                }
            }
        }

        private void LoadProperties(DynamicDeckItem item, FlowLayoutPanel panel)
        {
            var props = item.DeckAction.GetType().GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(ActionPropertyIncludeAttribute)) && TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom
                (typeof(string)));
            foreach (var prop in props) {
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
                        //After loosing focus, convert type to thingy.
                        prop.SetValue(item.DeckAction, TypeDescriptor.GetConverter(prop.PropertyType).ConvertFrom(txt.Text));
                    } catch (Exception ex) {
                        //Ignore all errors
                    }
                };
                txt.Width = panel.DisplayRectangle.Width - 16;
                panel.Controls.Add(txt);
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
    }
}