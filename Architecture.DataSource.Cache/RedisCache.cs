using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Architecture.Domain.Common.Cache;
using LanguageExt;
using Microsoft.Extensions.Caching.Distributed;

namespace Architecture.DataSource.Cache
{
    public class RedisCache : ICache
    {
        private readonly string _cacheKey;
        private readonly IDistributedCache _cache;

        public RedisCache(string cacheKey, IDistributedCache cache)
        {
            _cacheKey = cacheKey;
            _cache = cache;
        }

        public async Task<Either<CacheFailure, Option<T>>> GetAsync<T>(CancellationToken token)
        {
            return (await CacheHelper.GetBytes(() => _cache.GetAsync(_cacheKey, token)))
                .BindT(CacheHelper.DecodeBytesToString)
                .BindT(CacheHelper.DeserializeStringToObject<T>);
        }
    }
}