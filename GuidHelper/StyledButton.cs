using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuidHelper
{
    public class StyledButton : Button
    {
        private bool IsMouseOver => GetFlag(0x0001);
        private bool IsMouseDown => GetFlag(0x0002);
        private bool ShowFocusRect => ShowFocusCues && Focused;


        private bool GetFlag(int flag)
        {
            var dynMethod = typeof(ButtonBase).GetMethod("GetFlag", BindingFlags.NonPublic | BindingFlags.Instance);
            return (bool)dynMethod.Invoke(this, new object[] { flag });
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;

            Color cGradientStart, cGradientEnd, cBorder, cText, cTextShadow, cHighlight, cEdge, cGlow;
            int iTextShadowOffset, iGlowSize;
            if (IsMouseDown)
            {
                cGradientStart = Color.FromArgb(255, 216, 18);
                cGradientEnd = Color.FromArgb(252, 222, 68);
                cBorder = Color.FromArgb(153, 128, 0);
                cText = Color.FromArgb(0, 0, 0);
                cTextShadow = Color.FromArgb(192, 255, 255, 255);
                cHighlight = Color.FromArgb(32, 61, 39, 1);
                cEdge = Color.FromArgb(64, 255, 255, 255);
                cGlow = Color.FromArgb(72, 255, 255, 255);
                iTextShadowOffset = 1;
                iGlowSize = 4;
            }
            else if (IsMouseOver)
            {
                cGradientStart = Color.FromArgb(51, 139, 255);
                cGradientEnd = Color.FromArgb(28, 125, 252);
                cBorder = Color.FromArgb(10, 52, 107);
                cText = Color.FromArgb(255, 255, 255);
                cTextShadow = Color.FromArgb(128, 0, 0, 0);
                cHighlight = Color.FromArgb(128, 255, 255, 255);
                cEdge = Color.FromArgb(32, 2, 26, 59);
                cGlow = Color.FromArgb(96, 255, 255, 255);
                iTextShadowOffset = -1;
                iGlowSize = 6;
            }
            else
            {
                cGradientStart = Color.FromArgb(242, 248, 250);
                cGradientEnd = Color.FromArgb(216, 225, 230);
                cBorder = Color.FromArgb(157, 165, 168);
                cText = Color.FromArgb(0, 0, 0);
                cTextShadow = Color.FromArgb(192, 255, 255, 255);
                cHighlight = Color.FromArgb(128, 255, 255, 255);
                cEdge = Color.FromArgb(24, 19, 33, 38);
                cGlow = Color.FromArgb(72, 255, 255, 255);
                iTextShadowOffset = 1;
                iGlowSize = 4;
            }

            using (var b = new LinearGradientBrush(ClientRectangle, cGradientStart, cGradientEnd, 90f))
                g.FillRectangle(b, ClientRectangle);
            DrawGlow(g, ClientRectangle, cGlow, iGlowSize);
            using (var b = new SolidBrush(cHighlight))
                e.Graphics.FillRectangle(b, 0, 0, Width, 2);
            using (var b = new SolidBrush(cEdge))
                e.Graphics.FillRectangle(b, 0, Height - 2, Width, 2);
            using (var p = new Pen(cBorder, 1) { Alignment = PenAlignment.Inset })
                DrawRectangle(g, p, ClientRectangle);

            if (Image != null)
            {
                var iX = (Width - Image.Width) / 2;
                var iY = (Height - Image.Height) / 2;
                g.DrawImage(Image, iX, iY, Image.Width, Image.Height);
            }

            DrawText(g, Text, Font, OffsetRect(ClientRectangle, 0, iTextShadowOffset), cTextShadow);
            DrawText(g, Text, Font, ClientRectangle, cText);

            if (ShowFocusRect)
            {
                var focusRect = new Rectangle(4, 4, Width - 8, Height - 8);
                DrawRectangle(g, Pens.White, focusRect);
                using (var p = new Pen(Color.Black, 1) { DashStyle = DashStyle.Dot })
                    DrawRectangle(g, p, focusRect);
            }
        }

        private void DrawText(Graphics g, string text, Font font, Rectangle rect, Color color)
        {
            using (var b = new SolidBrush(color))
            using (var sf = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter,
                FormatFlags = StringFormatFlags.NoWrap,
                HotkeyPrefix = ShowKeyboardCues ? HotkeyPrefix.Show : HotkeyPrefix.Hide
            })
            {
                g.DrawString(text, font, b, rect, sf);
            }
        }

        private static Rectangle OffsetRect(Rectangle rect, int x, int y)
        {
            return new Rectangle(rect.X + x, rect.Y + y, rect.Width, rect.Height);
        }

        private static void DrawRectangle(Graphics g, Pen p, Rectangle r)
        {
            var pw = p.Width;
            var rect = new Rectangle(r.Left, r.Top, r.Width - (int)pw, r.Height - (int)pw);
            g.DrawRectangle(p, rect.Left, rect.Top, rect.Width, rect.Height);
        }

        public static int Remap(int value, int low1, int high1, int low2, int high2)
        {
            return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
        }

        private static void DrawGlow(Graphics g, Rectangle rect, Color color, int size)
        {
            for (var i = 0; i < size; ++i)
            {
                var a = Remap(i, 0, size, color.A, 0);

                var newRect = new Rectangle(rect.X + i, rect.Y + i, rect.Width - 2 * i, rect.Height - 2 * i);
                using (var p = new Pen(Color.FromArgb(a, color), 1f) { Alignment = PenAlignment.Inset })
                    DrawRectangle(g, p, newRect);
            }
        }
    }
}
