using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CourseProjectDataBaseCars
{
    public class CarComparer : IEqualityComparer<Car>
    {
        public bool Equals([AllowNull] Car x, [AllowNull] Car y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] Car obj)
        {
            return base.GetHashCode();
        }
    }
}
