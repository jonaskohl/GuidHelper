using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GuidHelper
{
    public partial class RForm : Form
    {
        StyledButton_ b;

        public RForm()
        {
            InitializeComponent();

            b = new StyledButton_();
            b.Text = "Download";
            b.Font = new Font("Arial", 16, FontStyle.Bold | FontStyle.Italic);
            //b.Size = new Size(72, 23);
            b.AutoSize = true;
            b.Location = Point.Empty;
            Controls.Add(b);

            Shown += RForm_Shown;
        }

        private void RForm_Shown(object sender, EventArgs e)
        {
            b.Invalidate();
            using (var i = new Bitmap(b.Width, b.Height))
            {
                b.DrawToBitmap(i, b.ClientRectangle);
                i.Save("BUTTON_" + b.Text + "_0.BMP", ImageFormat.Bmp);
            }
            b.State = ButtonState.Over;
            b.Invalidate();
            using (var i = new Bitmap(b.Width, b.Height))
            {
                b.DrawToBitmap(i, b.ClientRectangle);
                i.Save("BUTTON_" + b.Text + "_1.BMP", ImageFormat.Bmp);
            }
            b.State = ButtonState.Pressed;
            b.Invalidate();
            using (var i = new Bitmap(b.Width, b.Height))
            {
                b.DrawToBitmap(i, b.ClientRectangle);
                i.Save("BUTTON_" + b.Text + "_2.BMP", ImageFormat.Bmp);
            }
            Close();
        }
    }
}
