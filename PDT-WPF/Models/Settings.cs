﻿namespace PDT_WPF.Models
{
    public class Settings
    {
        public string OpenId { get; set; }
        public int UserId { get; set; }


        /// <summary>
        /// 默认设置
        /// </summary>
        public static Settings Default
        {
            get => new Settings
            {
                OpenId = string.Empty
            };
        }
    }
}
