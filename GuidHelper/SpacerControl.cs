using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GuidHelper
{
    public class SpacerControl : Control
    {
        public new bool CanFocus => false;

        public SpacerControl()
        {
            SetStyle(ControlStyles.Selectable, false);
            TabStop = false;
        }
    }
}
