namespace Stocks.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool AnyWhenNull<T>(this IEnumerable<T> collection)
            => collection?.Any() ?? false;
    }
}
