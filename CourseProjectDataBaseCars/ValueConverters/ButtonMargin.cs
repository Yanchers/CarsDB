using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;

namespace CourseProjectDataBaseCars
{
    class ButtonMargin : BaseValueConverter<ButtonMargin>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var newThickness = new Thickness(0, 0, (double)value, 0);
            return newThickness;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
