using Microsoft.Extensions.Logging;

namespace Architecture.Utils.Extensions
{
    public static class GenericExtensions
    {
        public static T LogInfo<T>(this T @this, ILogger logger)
        {
            logger.LogInformation(@this.ToString());
            return @this;
        }
    }
}