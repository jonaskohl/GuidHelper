using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuidHelper
{
    public static class Formats
    {
        public static readonly string[] FormatsList = new string[]
        {
            "%xA-%xB-%xC-%xD-%xE",
            "%XA-%XB-%XC-%XD-%XE",
            "{%xA-%xB-%xC-%xD-%xE}",
            "{%XA-%XB-%XC-%XD-%XE}",
            "(%xA-%xB-%xC-%xD-%xE)",
            "(%XA-%XB-%XC-%XD-%XE)",
            "\"%xA-%xB-%xC-%xD-%xE\"",
            "\"%XA-%XB-%XC-%XD-%XE\"",
            "%x#",
            "%X#",
            "new Guid(new byte[] { 0x%xA, 0x%xB, 0x%xC, 0x%xD%xE })",
            "[Guid(\"%XA-%XB-%XC-%XD-%XE\")]"
        };
    }
}
