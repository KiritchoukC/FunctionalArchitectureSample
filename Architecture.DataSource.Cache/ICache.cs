using System.Threading.Tasks;
using Architecture.Domain.Common.Cache;
using LanguageExt;

namespace Architecture.DataSource.Cache
{
    public interface ICache<T>
    {
        EitherAsync<CacheFailure, Option<T>> GetAsync();
        Either<CacheFailure, Unit> Set(T item);
    }
}