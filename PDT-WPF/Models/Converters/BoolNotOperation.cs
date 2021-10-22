using System.Globalization;

namespace PDT_WPF.Models.Converters
{
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
