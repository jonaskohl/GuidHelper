using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuidHelper
{
    public static class Formatters
    {
        public static readonly GuidFormatter[] FormattersList = new GuidFormatter[]
        {
            (Guid g) => g.ToString(),
            (Guid g) => g.ToString().ToUpper(),
            (Guid g) => "{" + g.ToString() + "}",
            (Guid g) => "{" + g.ToString().ToUpper() + "}",
            (Guid g) => "(" + g.ToString() + ")",
            (Guid g) => "(" + g.ToString().ToUpper() + ")",
            (Guid g) => "\"" + g.ToString() + "\"",
            (Guid g) => "\"" + g.ToString().ToUpper() + "\"",
            (Guid g) => g.ToString().Replace("-", ""),
            (Guid g) => g.ToString().ToUpper().Replace("-", ""),
            (Guid g) =>
            {
                var bytes = g.ToString("D").ToUpper();
                var a = "0x" + bytes.Substring(0, 8);
                var b = "0x" + bytes.Substring(9, 4);
                var c = "0x" + bytes.Substring(14, 4);
                var ds = bytes.Substring(19, 4) + bytes.Substring(24);
                var d = "new byte[] { " + string.Join(", ", Util.SpliceText(ds, 2).Select(i => "0x" + i)) + "}";
                return "new Guid(" + string.Join(", ", a, b, c, d) + ")";
            },
            (Guid g) => "[Guid(\"" + g.ToString().ToUpper() + "\")]"
        };
    }
}
