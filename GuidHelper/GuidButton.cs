using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuidHelper
{
    public class GuidButton : StyledButton
    {
        public Guid Guid;
        public GuidFormatter Formatter;

        public void RefreshText()
        {
            Text = Formatter(Guid);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Clipboard.SetText(Formatter(Guid));
            MessageBox.Show("Copied GUID to clipboard!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
