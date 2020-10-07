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
        Task<Either<CacheFailure, Unit>> SetAsync<T>(T item, CancellationToken token);
    }
}