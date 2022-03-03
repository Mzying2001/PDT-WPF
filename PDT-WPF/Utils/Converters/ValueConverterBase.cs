using System;
using System.Globalization;
using System.Windows.Data;

namespace PDT_WPF.Utils.Converters
{
    public abstract class ValueConverterBase<TIn, TOut> : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert((TIn)value, parameter, culture);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertBack((TOut)value, parameter, culture);
        }

        public abstract TOut Convert(TIn value, object parameter, CultureInfo culture);
        public abstract TIn ConvertBack(TOut value, object parameter, CultureInfo culture);
    }
}
