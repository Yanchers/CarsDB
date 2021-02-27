using System;
using System.Collections.Generic;

#nullable disable

namespace CourseProjectDataBaseCars
{
    public partial class Factory
    {
        public Factory()
        {
            CarsFactories = new HashSet<CarsFactory>();
        }

        public Factory(Factory f)
        {
            Id = f.Id;
            Country = f.Country;
            City = f.City;
            TranspCost = f.TranspCost;
            DeliveryTime = f.DeliveryTime;
            Type = f.Type;
            CarsFactories = new HashSet<CarsFactory>(f.CarsFactories);
        }

        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public decimal TranspCost { get; set; }
        public int DeliveryTime { get; set; }
        public TransportationTypes Type { get; set; }

        public virtual ICollection<CarsFactory> CarsFactories { get; set; }
    }
}
