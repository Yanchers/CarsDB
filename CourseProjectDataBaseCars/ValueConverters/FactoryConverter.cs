﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CourseProjectDataBaseCars
{
    public class FactoryConverter : BaseValueConverter<FactoryConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((List<Factory>)value).Select(f => f.Country + " " + f.City).ToList();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
