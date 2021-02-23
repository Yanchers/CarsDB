using System;
using System.Collections.Generic;

#nullable disable

namespace CourseProjectDataBaseCars
{
    public partial class Car
    {
        public Car()
        {
            CarsCredits = new HashSet<CarsCredit>();
            CarsFactories = new HashSet<CarsFactory>();
        }
        public Car(Car c)
        {
            Id = c.Id;
            Name = c.Name;
            Cost = c.Cost;
            CarsCredits = new HashSet<CarsCredit>(c.CarsCredits);
            CarsFactories = new HashSet<CarsFactory>(c.CarsFactories);
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }

        public virtual ICollection<CarsCredit> CarsCredits { get; set; }
        public virtual ICollection<CarsFactory> CarsFactories { get; set; }
    }
}
