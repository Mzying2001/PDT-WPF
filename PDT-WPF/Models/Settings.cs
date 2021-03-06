namespace PDT_WPF.Models
{
    public class Settings
    {
        public string OpenId { get; set; }
        public bool EnableLog { get; set; }
        public WindowSizeInfo MainWindowSizeInfo { get; set; }


        /// <summary>
        /// 默认设置
        /// </summary>
        public static Settings Default
        {
            get => new Settings
            {
                OpenId = string.Empty,
#if DEBUG
                EnableLog = true,
#else
                EnableLog = false,
#endif
            };
        }
    }
}
