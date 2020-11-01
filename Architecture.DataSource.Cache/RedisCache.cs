namespace Architecture.DataSource.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Architecture.Domain.Common.Cache;
    using Architecture.Utils.Extensions;
    using LanguageExt;
    using Microsoft.Extensions.Caching.Distributed;
    using static LanguageExt.Prelude;

    public class RedisCache<T> : ICache<T>
    {
        private readonly string _cacheKey;
        private readonly IDistributedCache _cache;

        public RedisCache(string cacheKey, IDistributedCache cache)
        {
            _cacheKey = cacheKey;
            _cache = cache;
        }

        public async Task<Either<CacheFailure, Option<T>>> GetAsync()
        {
            var fetchBytes = fun(async () => await CacheHelper.GetBytes(async () => await _cache.GetAsync(_cacheKey)));

            var decodeBytes = fun((byte[] bytes) => CacheHelper.DecodeBytesToString(bytes).Map(Optional));

            var deserializeString = fun((string json) => CacheHelper.DeserializeStringToObject<T>(json).Map(Optional));

            return (await fetchBytes())
                .BindT(decodeBytes)
                .BindT(deserializeString);
        }

        public Either<CacheFailure, Unit> Set(T item) =>
            CacheHelper.SerializeObjectToString(item)
                .Bind(CacheHelper.EncodeStringToBytes)
                .Bind(bytes => CacheHelper.SetBytes(() => _cache.Set(_cacheKey, bytes)));
    }
}