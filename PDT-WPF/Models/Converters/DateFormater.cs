using System;
using System.Globalization;

namespace PDT_WPF.Models.Converters
{
    public class DateFormater : ValueConverterBase<string, string>
    {
        public override string Convert(string value, object parameter, CultureInfo culture)
        {
            try
            {
                DateTime d = DateTime.Parse(value);

                DateTime now = DateTime.Now;
                TimeSpan gap = now - d;

                if (gap.TotalSeconds < 60)
                {
                    return "刚刚";
                }
                else if (gap.TotalMinutes < 60)
                {
                    return $"{gap.Minutes}分钟前";
                }
                else if (gap.TotalHours < 24)
                {
                    return $"{gap.Hours}小时前";
                }
                else if ((int)gap.TotalDays == 1)
                {
                    return "昨天";
                }
                else if ((int)gap.TotalDays == 2)
                {
                    return "前天";
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
            catch
            {
                return "???";
            }
        }

        public override string ConvertBack(string value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
