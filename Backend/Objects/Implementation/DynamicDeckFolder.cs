using NickAc.Backend.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NickAc.Backend.Objects.Implementation
{
    [Serializable]
    public class DynamicDeckFolder : IDeckFolder
    {
        public DeckImage DeckImage { get; set; }
        SerializableDictionary<int, IDeckItem> items = new SerializableDictionary<int, IDeckItem>();

        public SerializableDictionary<int, IDeckItem> Items {
            get {
                return items;
            }
            set {
                items = value;
            }
        }

        public override List<IDeckItem> GetDeckItems() => items.Values.ToList();
        public override List<IDeckFolder> GetSubFolders() => items.Values.OfType<IDeckFolder>().ToList();

        public override void Add(int slot, IDeckItem item) => items[slot] = item;

        public override DeckImage GetItemImage()
        {
            return DeckImage;
        }

        public override int GetItemIndex(IDeckItem item)
        {
            if (!items.Any(c => c.Value == item)) return -1;
            var key = items.First(c => c.Value == item);
            return key.Key;
        }

        [XmlIgnore]
        public IDeckFolder ParentFolder { get; set; }
        public override IDeckFolder GetParent()
        {
            return ParentFolder;
        }

        public override void SetParent(IDeckFolder folder)
        {
            ParentFolder = folder;
        }

        public override int Add(IDeckItem item)
        {
            int addToSlot = -1;
            for (int i = 15 - 1; i >= 0; i--) {
                if (items.ContainsKey(i)) {
                    addToSlot = i + 1;
                    break;
                }
            }
            if (addToSlot != -1) {
                Add(addToSlot, item);
            }
            return addToSlot;
        }
    }
}
