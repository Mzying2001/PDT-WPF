﻿using System;
using System.Globalization;
using System.Linq;

namespace PDT_WPF.Models.Converters
{
    public class StringsNotAllEmpty : MultiValueConverterBase<string, bool>
    {
        public override bool Convert(string[] values, object parameter, CultureInfo culture)
        {
            return values.All(i => !string.IsNullOrWhiteSpace(i));
        }

        public override string[] ConvertBack(bool value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
