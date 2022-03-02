using System.IO;

namespace PDT_WPF.Utils
{
    /// <summary>
    /// 处理文件
    /// </summary>
    public static class FileUtility
    {
        /// <summary>
        /// 获取路径对应文件的文件名
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileName(string filePath)
        {
            int index = filePath.Replace('/', '\\').LastIndexOf('\\');
            return index == -1 ? filePath : filePath.Substring(index + 1);
        }

        /// <summary>
        /// 以二进制的形式读取文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] ReadBytes(string filePath)
        {
            byte[] arr = null;
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                arr = new byte[fs.Length];
                fs.Read(arr, 0, arr.Length);
            }
            return arr;
        }
    }
}
