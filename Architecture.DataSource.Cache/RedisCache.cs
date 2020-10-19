using System;
using System.Collections.Generic;
using Architecture.Domain.Common.Cache;
using LanguageExt;
using Microsoft.Extensions.Caching.Distributed;

using static LanguageExt.Prelude;

namespace Architecture.DataSource.Cache
{
    public class RedisCache<T> : ICache<T>
    {
        private readonly string _cacheKey;
        private readonly IDistributedCache _cache;

        public RedisCache(string cacheKey, IDistributedCache cache)
        {
            _cacheKey = cacheKey;
            _cache = cache;
        }

        public Either<CacheFailure, Option<T>> Get()
        {
            return CacheHelper.GetBytes(() => _cache.Get(_cacheKey).Apply(Optional))
                .BindT(CacheHelper.DecodeBytesToString)
                .BindT(CacheHelper.DeserializeStringToObject<T>);
        }

        public Either<CacheFailure, Unit> Set(T item)
        {
            return CacheHelper.SerializeObjectToString(item)
                .Bind(CacheHelper.EncodeStringToBytes)
                .Bind(bytes => CacheHelper.SetBytes(() => _cache.Set(_cacheKey, bytes)));
        }
    }
}