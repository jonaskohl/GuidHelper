using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace GuidHelper
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();

            var fvi = FileVersionInfo.GetVersionInfo(Application.ExecutablePath);

            var sb = new StringBuilder();
            sb.AppendLine(fvi.Comments);
            sb.AppendLine("Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
            sb.AppendLine(fvi.LegalCopyright);

            label2.Text = sb.ToString();
        }
    }
}
