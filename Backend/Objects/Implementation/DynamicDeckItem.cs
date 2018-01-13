using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NickAc.Backend.Objects.Implementation
{
    [Serializable]
    public class DynamicDeckItem : IDeckItem
    {
        public override DeckImage GetDefaultImage()
        {
            return DeckAction.GetDefaultItemImage();
        }

        public DeckImage DeckImage { get; set; }
        public AbstractDeckAction DeckAction { get; set; }

        public override DeckImage GetItemImage()
        {
            return DeckImage;
        }
    }
}
