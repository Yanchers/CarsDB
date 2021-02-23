using System;
using System.Collections.Generic;

#nullable disable

namespace CourseProjectDataBaseCars
{
    public partial class Credit
    {
        public Credit()
        {
            CarsCredits = new HashSet<CarsCredit>();
        }

        public int Id { get; set; }
        public int BankId { get; set; }
        public double Rate { get; set; }
        public int Expiration { get; set; }

        public virtual Bank Bank { get; set; }
        public virtual ICollection<CarsCredit> CarsCredits { get; set; }
    }
}
