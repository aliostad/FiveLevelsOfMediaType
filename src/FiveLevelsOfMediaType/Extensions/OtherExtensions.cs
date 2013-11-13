using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveLevelsOfMediaType
{
    internal static class OtherExtensions
    {
        public static bool IsIn<T>(this T t, IEnumerable<T> enumerable)
        {
            return enumerable.Any(x => x.Equals(t));
        }
    }
}
