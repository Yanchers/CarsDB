using System;
using System.Collections.Generic;

#nullable disable

namespace CourseProjectDataBaseCars
{
    public partial class CarsFactory
    {
        public int CarId { get; set; }
        public int FactoryId { get; set; }

        public virtual Car Car { get; set; }
        public virtual Factory Factory { get; set; }
    }
}
