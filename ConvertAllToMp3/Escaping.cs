using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllToMp3
{
    public static class Escaping
    {
        public static string ShellEncode(params string[] args)
        {
            return ShellEncode((IEnumerable<string>)args);
        }

        public static string ShellEncode(IEnumerable<string> args)
        {
            if (args == null)
                return String.Empty;

            var sb = new StringBuilder();

            foreach (string arg in args)
            {
                if (sb.Length > 0)
                    sb.Append(" ");

                sb.Append(ShellEncode(arg));
            }

            return sb.ToString();
        }

        public static string ShellEncode(string arg)
        {
            if (String.IsNullOrEmpty(arg))
                return "";

            return "\"" + arg.Replace("\"", "\"\"") + "\"";
        }
    }
}
