using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CourseProjectDataBaseCars
{
    public class FactoryComparer : IEqualityComparer<Factory>
    {
        public bool Equals([AllowNull] Factory x, [AllowNull] Factory y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] Factory obj)
        {
            return base.GetHashCode();
        }
    }
}
