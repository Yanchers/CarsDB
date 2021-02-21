using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace CourseProjectDataBaseCars
{
    public partial class Car
    {
        public Car()
        {

        }
        public Car(Car car)
        {
            Id = car.Id;
            Name = car.Name;
            Cost = car.Cost;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
    }
}
