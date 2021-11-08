using System;
using System.Globalization;

namespace PDT_WPF.Models.Converters
{
    public class BoardPhotoJumpTypeToString : ValueConverterBase<BoardPhoto.JumpType, string>
    {
        public override string Convert(BoardPhoto.JumpType value, object parameter, CultureInfo culture)
        {
            return BoardPhoto.GetJumpTypeName(value);
        }

        public override BoardPhoto.JumpType ConvertBack(string value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
