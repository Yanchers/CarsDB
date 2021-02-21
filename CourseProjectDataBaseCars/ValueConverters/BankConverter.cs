using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProjectDataBaseCars
{
    public class BankConverter : BaseValueConverter<BankConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((List<Bank>)value).Select(b=>b.Name).ToList();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value;
        }
    }
}
