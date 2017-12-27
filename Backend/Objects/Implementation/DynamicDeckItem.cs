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
        enum DeckItemType
        {
            Process,
            HotkeyToggle,
            HotKeyPress
        }

        DeckItemType ItemType { get; set; }
    }
}
