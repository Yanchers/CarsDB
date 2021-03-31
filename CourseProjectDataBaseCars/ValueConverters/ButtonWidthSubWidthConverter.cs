﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;

namespace CourseProjectDataBaseCars
{
    class ButtonWidthSubWidthConverter : BaseValueConverter<ButtonWidthSubWidthConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var newThickness = new Thickness(0, 0, -((Thickness)value).Right, 0);
            return newThickness;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}