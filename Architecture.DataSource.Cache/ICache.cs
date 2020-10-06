using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Architecture.Domain.Common.Cache;
using LanguageExt;

namespace Architecture.DataSource.Cache
{
    public interface ICache
    {
        Task<Either<CacheFailure, Option<T>>> GetAsync<T>(CancellationToken token);
        T SetAsync<T>(string cacheKey, T item, CancellationToken token);
    }
}