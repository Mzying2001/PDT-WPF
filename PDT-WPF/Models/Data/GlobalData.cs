namespace PDT_WPF.Models.Data
{
    /// <summary>
    /// 全局数据
    /// </summary>
    public static class GlobalData
    {
        public static User CurrentUser { get; set; }
        public static bool ShowAdminPage { get; set; }
        public static bool AdminMode { get; set; }

        static GlobalData()
        {

#if DEBUG
            //Debug编译模式下默认开启ShowAdminPage
            ShowAdminPage = true;
#endif

        }
    }
}
