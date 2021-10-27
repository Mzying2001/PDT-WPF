using System.Collections.Generic;

namespace PDT_WPF.Services.Api
{
    /// <summary>
    /// Pdt接口共用数据
    /// </summary>
    public static class PdtCommon
    {
        public static Dictionary<string, string> Headers { get; } = new Dictionary<string, string>
        {
            ["Token"] = string.Empty
        };

        public static string Token
        {
            get => Headers["Token"];
            set => Headers["Token"] = value;
        }



        /// <summary>
        /// 后台管理相关API的Headers
        /// </summary>
        public static Dictionary<string, string> AdminApiHeaders { get; } = new Dictionary<string, string>
        {
            ["Token"] = string.Empty
        };

        /// <summary>
        /// 后台管理相关API的Token
        /// </summary>
        public static string AdminApiToken
        {
            get => AdminApiHeaders["Token"];
            set => AdminApiHeaders["Token"] = value;
        }
    }
}
