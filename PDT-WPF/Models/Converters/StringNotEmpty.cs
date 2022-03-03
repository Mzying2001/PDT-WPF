using System;
using System.Globalization;

namespace PDT_WPF.Models.Converters
{
    public class StringNotEmpty : ValueConverterBase<string, bool>
    {
        public override bool Convert(string value, object parameter, CultureInfo culture)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public override string ConvertBack(bool value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
