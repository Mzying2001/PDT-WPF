using System.Windows;

namespace PDT_WPF.Models
{
    /// <summary>
    /// 窗口尺寸和位置信息
    /// </summary>
    public class WindowSizeInfo
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }


        /// <summary>
        /// 获取窗口尺寸和位置信息
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public static WindowSizeInfo GetSizeInfo(Window window)
        {
            return new WindowSizeInfo
            {
                Left = window.Left,
                Top = window.Top,
                Width = window.Width,
                Height = window.Height
            };
        }

        /// <summary>
        /// 设置窗口尺寸和位置
        /// </summary>
        /// <param name="window"></param>
        public void Apply(Window window)
        {
            window.Left = Left;
            window.Top = Top;
            window.Width = Width;
            window.Height = Height;
        }
    }
}
