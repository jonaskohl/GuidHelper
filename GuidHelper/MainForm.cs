using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuidHelper
{
    public partial class MainForm : Form
    {
        GuidButton[] buttonList;
        Guid currentGuid;
        Font guidButtonFont;

        const int WM_NCHITTEST = 0x84;
        enum HitTest : int
        {
            Caption = 2,
            Transparent = -1,
            Nowhere = 0,
            Client = 1,
            Left = 10,
            Right = 11,
            Top = 12,
            TopLeft = 13,
            TopRight = 14,
            Bottom = 15,
            BottomLeft = 16,
            BottomRight = 17,
            Border = 18
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    var result = (HitTest)m.Result.ToInt32();
                    if (result == HitTest.Left || result == HitTest.Right)
                        m.Result = new IntPtr((int)HitTest.Caption);
                    if (result == HitTest.TopLeft || result == HitTest.TopRight)
                        m.Result = new IntPtr((int)HitTest.Top);
                    if (result == HitTest.BottomLeft || result == HitTest.BottomRight)
                        m.Result = new IntPtr((int)HitTest.Bottom);

                    break;
            }
        }

        void LoadDefaultFormats()
        {
            buttonList = CreateButtonsForFormats(Formats.FormatsList);
        }

        public MainForm()
        {
            InitializeComponent();

            guidButtonFont = new Font("Courier New", 8);

            string formatsFilePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "GUIDFormats.txt");
            if (File.Exists(formatsFilePath))
            {
                try
                {
                    buttonList = CreateButtonsForFormats(File.ReadAllLines(formatsFilePath).Where(x => !string.IsNullOrEmpty(x)).ToArray());
                }
                catch (FormatException)
                {
                    MessageBox.Show("Format list contains invalid GUID formats!", "GUID Helper", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    LoadDefaultFormats();
                }
            }
            else
            {
                LoadDefaultFormats();
            }

            panel1.SuspendLayout();
            panel1.Controls.AddRange(
                buttonList
                    .Select((b, i) => { b.TabIndex = i; return b; })
                    .Reverse()
                    .SelectMany(b => new Control[] { b, new SpacerControl() { Dock = DockStyle.Top, Height = 6 } })
                    .ToArray());
            panel1.ResumeLayout(true);

            NewGuid();
        }

        private void NewGuid()
        {
            currentGuid = Guid.NewGuid();
            RefreshTexts();
        }

        private void RefreshTexts()
        {
            foreach (var b in buttonList)
            {
                b.Guid = currentGuid;
                b.RefreshText();
            }
        }

        public GuidButton[] CreateButtonsForFormatters(params GuidFormatter[] formatters)
        {
            return formatters.Select(f => new GuidButton()
            {
                Formatter = f,
                Dock = DockStyle.Top,
                Font = guidButtonFont,
                UseVisualStyleBackColor = true
            }).ToArray();
        }
        public GuidButton[] CreateButtonsForFormats(params string[] formats)
        {
            return formats.Select(f => new GuidButton()
            {
                GuidFormatString = f,
                Dock = DockStyle.Top,
                Font = guidButtonFont,
                UseVisualStyleBackColor = true
            }).ToArray();
        }

        private void newGuidButton_Click(object sender, EventArgs e)
        {
            NewGuid();
        }

        private void aboutButton_Click(object sender, EventArgs e)
        {
            using (var f = new AboutForm())
                f.ShowDialog(this);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            /*
            var bounds = new Rectangle(0, - panel3.Height / 2, panel3.Width, panel3.Height);
            using (var ellipsePath = new GraphicsPath())
            {
                ellipsePath.AddEllipse(bounds);
                using (var brush = new PathGradientBrush(ellipsePath))
                {
                    brush.CenterPoint = new PointF(bounds.Width / 2f, bounds.Height / 2f);
                    brush.CenterColor = Color.FromArgb(220, 220, 220);
                    brush.SurroundColors = new[] { Color.White };
                    brush.FocusScales = new PointF(0, 0);

                    e.Graphics.FillRectangle(brush, bounds);
                }
            }

            var lbounds = new Rectangle(0, 0, Width, 1);
            var lcolor = Color.FromArgb(199, 199, 199);
            using (var b = new LinearGradientBrush(lbounds, Color.Black, Color.Black, 0f)
            {
                InterpolationColors = new ColorBlend()
                {
                    Colors = new[] { Color.FromArgb(0, lcolor), lcolor, Color.FromArgb(0, lcolor) },
                    Positions = new[] { 0.0f, 0.5f, 1.0f }
                }
            })
                e.Graphics.FillRectangle(b, lbounds);
            */

            using (var b = new LinearGradientBrush(panel3.ClientRectangle, Color.FromArgb(244, 245, 247), Color.FromArgb(209, 210, 212), 90f))
                e.Graphics.FillRectangle(b, panel3.ClientRectangle);

            using (var p = new Pen(Color.FromArgb(199, 199, 199), 1))
                e.Graphics.DrawLine(p, 0, 0, Width, 0);
        }
    }
}
