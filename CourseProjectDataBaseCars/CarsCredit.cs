using System;
using System.Collections.Generic;

#nullable disable

namespace CourseProjectDataBaseCars
{
    public partial class CarsCredit
    {
        public int CarId { get; set; }
        public int CreditId { get; set; }

        public virtual Car Car { get; set; }
        public virtual Credit Credit { get; set; }
    }
}
