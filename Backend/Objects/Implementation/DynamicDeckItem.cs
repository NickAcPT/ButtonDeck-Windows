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
        public AbstractDeckAction DeckAction { get; set; }
    }
}
