namespace PDT_WPF.Models.Data
{
    /// <summary>
    /// 全局数据
    /// </summary>
    public static class GlobalData
    {
        public static User CurrentUser { get; set; }
        public static bool AdminMode { get; set; }

        static GlobalData()
        {

        }
    }
}
