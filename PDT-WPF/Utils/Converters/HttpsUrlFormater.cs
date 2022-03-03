using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PDT_WPF.Utils.Converters
{
    public class HttpsUrlFormater : ValueConverterBase<string, string>
    {
        public override string Convert(string value, object parameter, CultureInfo culture)
        {
            return Regex.IsMatch(value, "^https?://.+$") ? value : $"https://{value}";
        }

        public override string ConvertBack(string value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
