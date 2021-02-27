using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CourseProjectDataBaseCars
{
    public class TranspEnumToStringConverter : BaseValueConverter<TranspEnumToStringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((TransportationTypes)value) switch
            {
                TransportationTypes.None => "Ничего",
                TransportationTypes.Plane => "Самолет",
                TransportationTypes.Car => "Машина",
                TransportationTypes.Ship => "Корабль",
                TransportationTypes.Train => "Ж/Д",
                _ => "Ничего",
            };
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value) switch
            {
                "Ничего" => TransportationTypes.None,
                "Самолет" => TransportationTypes.Plane,
                "Машина" => TransportationTypes.Car,
                "Корабль" => TransportationTypes.Ship,
                "Ж/Д" => TransportationTypes.Train,
                _ => "Ничего",
            };
        }
    }
}
