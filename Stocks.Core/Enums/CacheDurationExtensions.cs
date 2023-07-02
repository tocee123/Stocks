namespace Stocks.Core.Enums
{
    public static class CacheDurationExtensions
    {
        public static TimeSpan? GetExpiration(this CacheDuration cacheDuration)
        => cacheDuration switch
        {
            CacheDuration.Default or CacheDuration.OneHour => CreateTimespanFromSeconds(3600),
            CacheDuration.OneMinute => CreateTimespanFromSeconds(60),
            CacheDuration.Unlimited => null,
            _ => throw new NotImplementedException()
        };

        private static TimeSpan CreateTimespanFromSeconds(int seconds)
            => TimeSpan.FromSeconds(seconds);
    }
}
