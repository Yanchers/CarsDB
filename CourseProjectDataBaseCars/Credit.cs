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
        public Credit(Credit c)
        {
            Id = c.Id;
            BankId = c.BankId;
            Rate = c.Rate;
            Expiration = c.Expiration;

            Bank = c.Bank;
            CarsCredits = c.CarsCredits;
        }

        public int Id { get; set; }
        public int BankId { get; set; }
        public double Rate { get; set; }
        public int Expiration { get; set; }

        public virtual Bank Bank { get; set; }
        public virtual ICollection<CarsCredit> CarsCredits { get; set; }
    }
}
