using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuidHelper
{
    public class StyledButton_ : StyledButton
    {
        public ButtonState State;

        protected override bool IsMouseOver => State == ButtonState.Over;
        protected override bool IsMouseDown => State == ButtonState.Pressed;

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            base.OnPaint(e);
        }

    }

    public enum ButtonState
    {
        Normal,
        Over,
        Pressed
    }
}
