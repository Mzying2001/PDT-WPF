using System;
using System.Globalization;
using System.Windows;

namespace PDT_WPF.Utils.Converters
{
    /// <summary>
    /// 负数时不可见，非负时可见
    /// </summary>
    public class VisiableIfNotNegative : ValueConverterBase<int, Visibility>
    {
        public override Visibility Convert(int value, object parameter, CultureInfo culture)
        {
            return value >= 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public override int ConvertBack(Visibility value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
