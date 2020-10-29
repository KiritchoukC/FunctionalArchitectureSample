using System;
using Microsoft.Extensions.Logging;

namespace Architecture.Utils.Extensions
{
    public static class GenericExtensions
    {
        public static T LogInfo<T>(this T @this, ILogger logger, Func<T, string> messageFunc)
        {
            logger.LogInformation(messageFunc(@this));
            return @this;
        }
    }
}