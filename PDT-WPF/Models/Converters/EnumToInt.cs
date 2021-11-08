using System;
using System.Globalization;

namespace PDT_WPF.Models.Converters
{
    public class EnumToInt : ValueConverterBase<Enum, int>
    {
        public override int Convert(Enum value, object parameter, CultureInfo culture)
        {
            return (int)(object)value;
        }

        public override Enum ConvertBack(int value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
