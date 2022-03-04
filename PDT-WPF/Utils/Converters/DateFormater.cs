using System;
using System.Globalization;

namespace PDT_WPF.Utils.Converters
{
    public class DateFormater : ValueConverterBase<string, string>
    {
        private int GetGapDay(DateTime start, DateTime end)
        {
            DateTime a = new DateTime(start.Year, start.Month, start.Day);
            DateTime b = new DateTime(end.Year, end.Month, end.Day);
            return (int)(b - a).TotalDays;
        }

        public override string Convert(string value, object parameter, CultureInfo culture)
        {
            try
            {
                DateTime d = DateTime.Parse(value);

                DateTime now = DateTime.Now;
                TimeSpan gap = now - d;

                int gapDay = GetGapDay(d, now);

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
                else if (gapDay == 1)
                {
                    return "昨天";
                }
                else if (gapDay == 2)
                {
                    return "前天";
                }
                else if (gapDay <= 7)
                {
                    return $"{gapDay}天前";
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
