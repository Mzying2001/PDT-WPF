using Newtonsoft.Json.Linq;
using System.Text;

namespace PDT_WPF.Utils
{
    /// <summary>
    /// 用于展示错误信息时获取结构体中的所有值
    /// </summary>
    public static class ErrorFormater
    {
        public static string GetString<T>(T errors) where T : struct
        {
            var json = JObject.FromObject(errors);
            var sb = new StringBuilder();
            foreach (var item in json)
            {
                if (item.Value != null && !string.IsNullOrEmpty(item.Value.ToString()))
                    sb.AppendFormat("{0}: {1}\n", item.Key, item.Value.ToString());
            }
            return sb.ToString().Trim();
        }
    }
}
