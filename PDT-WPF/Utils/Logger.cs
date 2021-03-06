using PDT_WPF.Models.Data;
using System;
using System.IO;

namespace PDT_WPF.Utils
{
    internal static class Logger
    {
        private const string LogFileName = LocalData.DATA_PATH + "/Log.txt";

        private static bool opened;
        private static FileStream stream;
        private static StreamWriter writer;

        public static bool Opened => opened;

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

        public static void WriteLine(object value, string type = null)
        {
            if (!opened)
                return;

            lock (writer)
            {
                writer.Write(">> [{0}] ", DateTime.Now.ToString());
                writer.Write(type == null ? "" : $"[{type}] ");
                writer.WriteLine(value?.ToString() ?? "null");
                writer.WriteLine();
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
