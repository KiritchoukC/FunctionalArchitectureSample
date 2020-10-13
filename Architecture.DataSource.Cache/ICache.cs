using Architecture.Domain.Common.Cache;
using LanguageExt;

namespace Architecture.DataSource.Cache
{
    public interface ICache<T>
    {
        Either<CacheFailure, Option<T>> Get();
        Either<CacheFailure, Unit> Set(T item);
    }
}