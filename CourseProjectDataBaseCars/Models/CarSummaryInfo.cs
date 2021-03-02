using System;
using System.Collections.Generic;
using System.Text;

namespace CourseProjectDataBaseCars
{
    public class CarSummaryInfo
    {
        public int FactoryId { get; set; }
        public int CreditId { get; set; }
        public decimal CarCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal MonthlyPay { get; set; }
        public string Expiration { get; set; }
        public string BankName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public decimal TranspCost { get; set; }
        public string Arrival { get; set; }
    }
}
