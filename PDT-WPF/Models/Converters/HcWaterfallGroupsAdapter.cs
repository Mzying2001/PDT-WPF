using System;
using System.Globalization;

namespace PDT_WPF.Models.Converters
{
    /// <summary>
    /// 用于实现HandyControl的流式布局组数的自适应（ActualWidth -> Groups）
    /// </summary>
    public class HcWaterfallGroupsAdapter : ValueConverterBase<double, int>
    {
        public double ItemWidth { get; set; }

        public override int Convert(double value, object parameter, CultureInfo culture)
        {
            int tmp = (int)(value / ItemWidth);
            return tmp < 1 ? 1 : tmp;
        }

        public override double ConvertBack(int value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
