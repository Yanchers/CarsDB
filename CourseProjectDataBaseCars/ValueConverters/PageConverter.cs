using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProjectDataBaseCars
{
    public class PageConverter : BaseValueConverter<PageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((PageTypes)value) switch
            {
                PageTypes.Menu => new MenuPage(),
                PageTypes.Dev => new DevPage(),
                PageTypes.Catalog => new Catalog(),
                PageTypes.Car => new CarPage(),
                _ => new MenuPage(),
            };
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
