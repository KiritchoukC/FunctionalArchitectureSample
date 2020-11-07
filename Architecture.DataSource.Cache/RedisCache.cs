namespace Architecture.DataSource.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Architecture.Domain.Common.Cache;
    using Architecture.Utils.Extensions;
    using LanguageExt;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Logging;
    using static LanguageExt.Prelude;
    using static Architecture.Utils.Functions.GenericFunctions;

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

        public async Task<Either<CacheFailure, Option<T>>> GetAsync()
            => (await CacheHelper.GetBytes(async () => await _cache.GetAsync(_cacheKey)))
                .MapO(CacheHelper.DecodeBytesToString)
                .MapO(CacheHelper.DeserializeStringToObject<T>);

        public Either<CacheFailure, Unit> Set(T item)
            => CacheHelper.SerializeObjectToString(item)
                .Bind(CacheHelper.EncodeStringToBytes)
                .Bind(bytes => CacheHelper.SetBytes(() => _cache.Set(_cacheKey, bytes)));


    }
}