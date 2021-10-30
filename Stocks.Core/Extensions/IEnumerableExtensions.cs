using System.Collections.Generic;
using System.Linq;

namespace Stocks.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool AnyWhenNull<T>(this IEnumerable<T> collection)
            => collection?.Any() ?? false;
    }
}
