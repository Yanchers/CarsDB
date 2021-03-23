using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CourseProjectDataBaseCars
{
    class ButtonWidthSubWidthConverter : BaseValueConverter<ButtonWidthSubWidthConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value - (double)parameter;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
