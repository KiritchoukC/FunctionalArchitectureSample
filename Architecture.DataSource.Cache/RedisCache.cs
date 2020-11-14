namespace Architecture.DataSource.Cache
{
    using Architecture.Domain.Common.Cache;
    using Architecture.Utils.Extensions;

    using LanguageExt;

    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Logging;

    public class RedisCache<T> : ICache<T>
    {
        private readonly ILogger _logger;
        private readonly string _cacheKey;
        private readonly IDistributedCache _cache;

        public RedisCache(
            string cacheKey,
            IDistributedCache cache,
            ILogger logger)
        {
            _cacheKey = cacheKey;
            _cache = cache;
            _logger = logger;
        }

        public EitherAsync<CacheFailure, Option<T>> GetAsync() =>
            CacheHelper.GetBytes(() => _cache.GetStringAsync(_cacheKey))
                .MapO(CacheHelper.DeserializeStringToObject<T>);

        public EitherAsync<CacheFailure, Unit> SetAsync(T item)
            => CacheHelper.SerializeObjectToString(item)
                .Bind(jsonStr => CacheHelper.SetBytes(() => _cache.SetStringAsync(_cacheKey, jsonStr)));
    }
}