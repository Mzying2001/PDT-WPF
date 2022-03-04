using System;
using System.Globalization;

namespace PDT_WPF.Utils.Converters
{
    public class IntNotNegative : ValueConverterBase<int, bool>
    {
        public override bool Convert(int value, object parameter, CultureInfo culture)
        {
            return value >= 0;
        }

        public override int ConvertBack(bool value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
