using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CourseProjectDataBaseCars
{
    public class CreditConverter : BaseValueConverter<CreditConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((List<Credit>)value).Select(c=> c.Bank.Name + " | " + c.Rate + "% | " + c.Expiration + " мес.").ToList();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
