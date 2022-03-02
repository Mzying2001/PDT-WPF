using System;
using System.IO;

namespace PDT_WPF.Utils
{
    internal static class Logger
    {
        private const string LogFileName = "./Log.txt";

        private static bool opened;
        private static FileStream stream;
        private static StreamWriter writer;

        public static bool Open()
        {
            try
            {
                stream = new FileStream(LogFileName, FileMode.Append);
                writer = new StreamWriter(stream, System.Text.Encoding.GetEncoding("GB2312"));
                opened = true;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void Close()
        {
            if (opened)
            {
                writer.Close();
                stream.Close();
                opened = false;
            }
        }

        private static string UnshiftTime(string value)
        {
            return string.Format(">> [{0}] {1}", DateTime.Now.ToString(), value);
        }

        public static void WriteLine(object value, string type = null)
        {
            if (!opened)
                return;

            if (type == null)
            {
                writer.WriteLine(UnshiftTime(value?.ToString() ?? "null"));
            }
            else
            {
                writer.WriteLine(UnshiftTime($"[{type}] {value?.ToString() ?? "null"}"));
            }
        }

        public static void WriteError(object value)
        {
            if (opened)
            {
                WriteLine(value, "ERROR");
            }
        }
    }
}
