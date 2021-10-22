using System;
using System.Globalization;

namespace PDT_WPF.Models.Converters
{
    public class DateFormater : ValueConverterBase<string, string>
    {
        public override string Convert(string value, object parameter, CultureInfo culture)
        {
            DateTime d = DateTime.Parse(value);

            DateTime now = DateTime.Now;
            TimeSpan gap = now - d;

            if (gap.TotalSeconds < 60)
            {
                return $"{(int)gap.TotalSeconds}秒前";
            }
            else if (gap.TotalMinutes < 60)
            {
                return $"{(int)gap.TotalMinutes}分钟前";
            }
            else if (gap.TotalHours < 24)
            {
                return $"{(int)gap.TotalHours}小时前";
            }
            else if (gap.TotalDays <= 7)
            {
                return $"{(int)gap.TotalDays}天前";
            }
            else
            {
                return d.ToString("yyyy/MM/dd");
            }
        }

        public override string ConvertBack(string value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
