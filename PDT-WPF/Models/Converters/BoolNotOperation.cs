using System;
using System.Globalization;

namespace PDT_WPF.Models.Converters
{
    [Obsolete("使用HandyControl的Boolean2BooleanReConverter替换")]
    public class BoolNotOperation : ValueConverterBase<bool, bool>
    {
        public override bool Convert(bool value, object parameter, CultureInfo culture)
        {
            return !value;
        }

        public override bool ConvertBack(bool value, object parameter, CultureInfo culture)
        {
            return !value;
        }
    }
}
