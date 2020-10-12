using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Architecture.Domain.Common.Cache;
using LanguageExt;

namespace Architecture.DataSource.Cache
{
    public interface ICache
    {
        Either<CacheFailure, Option<T>> Get<T>();
        Either<CacheFailure, Unit> Set<T>(T item);
    }
}