using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GuidHelper
{
    public static class Util
    {
        public static string[] SpliceText(string text, int lineLength)
        {
            return Regex.Matches(text, ".{1," + lineLength + "}").Cast<Match>().Select(m => m.Value).ToArray();
        }

        private static string FormatBytes(string formatType, params byte[] bytes)
        {
            var sb = new StringBuilder();

            foreach (var b in bytes)
            {
                if (formatType == "b")
                    sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
                else if (formatType == "x" || formatType == "X")
                    sb.Append(b.ToString(formatType + "2"));
                else if (formatType == "d")
                    sb.Append(b.ToString());
                else if (formatType == "D")
                    sb.Append(b.ToString().PadLeft(3, '0'));
                else
                    throw new FormatException();
            }

            return sb.ToString();
        }

        public static string FormatGuid(Guid guid, string format)
        {
            var bytes = guid.ToByteArray();
            return Regex.Replace(format, @"%([dDbxX])(\d+|#|A|B|C|D|E)", (Match m) =>
            {
                var formatType = m.Groups[1].Value;
                var indexStr = m.Groups[2].Value;
                var isNumeric = int.TryParse(indexStr, out int index);

                if (isNumeric)
                {
                    return FormatBytes(formatType, bytes[index]);
                } else if (indexStr == "#")
                {
                    return FormatBytes(formatType, bytes);
                } else
                {
                    switch (indexStr)
                    {
                        case "A":
                            return FormatBytes(formatType, bytes.Skip(0).Take(4).ToArray());
                        case "B":
                            return FormatBytes(formatType, bytes.Skip(4).Take(2).ToArray());
                        case "C":
                            return FormatBytes(formatType, bytes.Skip(6).Take(2).ToArray());
                        case "D":
                            return FormatBytes(formatType, bytes.Skip(8).Take(2).ToArray());
                        case "E":
                            return FormatBytes(formatType, bytes.Skip(10).ToArray());
                        default:
                            throw new FormatException();
                    }
                }
            });
        }
    }
}
