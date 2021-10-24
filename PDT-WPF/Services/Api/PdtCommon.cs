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
    }
}
