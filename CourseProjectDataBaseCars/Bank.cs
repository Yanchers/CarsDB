using System;
using System.Collections.Generic;

#nullable disable

namespace CourseProjectDataBaseCars
{
    public partial class Bank
    {
        public Bank()
        {
            Credits = new HashSet<Credit>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Credit> Credits { get; set; }
    }
}
