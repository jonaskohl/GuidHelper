using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        public MainForm()
        {
            InitializeComponent();

            guidButtonFont = new Font("Courier New", 8);

            buttonList = CreateButtonsForFormatters(Formatters.FormattersList);

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
            return formatters.Select(f => new GuidButton() {
                Formatter = f,
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
    }
}
