using System.Collections.Generic;

namespace PDT_WPF.Services.Api
{
    public static class PdtV2
    {
        public const string BASE_URL = "https://pdt.ojbk.me/api/v2/";

        public static Dictionary<string, string> Headers
        {
            get => Pdt.Headers;
        }

        public static string Token
        {
            get => Headers["Token"];
            set => Headers["Token"] = value;
        }
    }
}
