using NickAc.Backend.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NickAc.Backend.Utils
{
    public class DeckActionHelper
    {
        public DeckActionHelper(AbstractDeckAction deckAction)
        {
            DeckAction = deckAction;
        }

        public AbstractDeckAction DeckAction { get; set; }
    }
}
