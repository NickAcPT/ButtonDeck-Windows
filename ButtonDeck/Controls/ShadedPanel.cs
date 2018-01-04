using NickAc.ModernUIDoneRight.Forms;
using NickAc.ModernUIDoneRight.Objects;
using NickAc.ModernUIDoneRight.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ButtonDeck.Forms
{
    class ShadedPanel : Panel
    {
        bool hasPainted;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (!hasPainted) {
                hasPainted = true;
                Loaded();
            }
        }

        private void Loaded()
        {
            //The control was drawn.
            //This means we can add the drop shadow
            ShadowUtils.CreateDropShadow(this);
            if (Parent != null) {
                Parent.Invalidate();
            }
        }

        private ColorScheme _colorScheme;

        public ColorScheme ColorScheme {
            get {
                if (Parent != null && Parent is ModernForm frm)
                    return frm.ColorScheme;
                return _colorScheme; }
            set { _colorScheme = value; }
        }



    }
}
