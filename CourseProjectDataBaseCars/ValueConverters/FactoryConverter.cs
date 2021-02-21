using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            string city = ((string)value).Split(" ")[1];
            return city;
        }
    }
}
