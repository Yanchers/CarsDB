using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace CourseProjectDataBaseCars
{
    public partial class Factory
    {
        public Factory()
        {

        }
        public Factory(Factory f)
        {
            Id = f.Id;
            Country = f.Country;
            City = f.City;
            TranspCost = f.TranspCost;
            DeliveryTime = f.DeliveryTime;
            Type = f.Type;
        }

        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public decimal? TranspCost { get; set; }
        public int? DeliveryTime { get; set; }
        public int? Type { get; set; }
    }
}
