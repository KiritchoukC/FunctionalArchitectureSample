
namespace Architecture.Infrastructure
{
    using Architecture.Domain.Common.Cache;

    using LanguageExt;

    public interface ICache<T>
    {
        EitherAsync<CacheFailure, Option<T>> Get(string cacheKey);
        EitherAsync<CacheFailure, Unit> Set(string cacheKey, T item);
    }
}
