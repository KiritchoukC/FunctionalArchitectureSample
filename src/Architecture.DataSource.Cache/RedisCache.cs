namespace Architecture.DataSource.Cache
{
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Architecture.Domain.Common.Cache;
    using Architecture.Infrastructure;
    using Architecture.Utils;

    using LanguageExt;

    using Microsoft.Extensions.Caching.Distributed;

    using static Architecture.Utils.Constructors;
    using static LanguageExt.Prelude;

    public class RedisCache<T> : ICache<T>
    {
        private readonly IDistributedCache _cache;

        public RedisCache(IDistributedCache cache) => _cache = cache;

        public EitherAsync<CacheFailure, Option<T>> Get(string cacheKey) =>
            TryAsync(async () => (await _cache.GetStringAsync(cacheKey)).Apply(Optional))
                .ToEither()
                .MapLeft(CacheFailureCon.Fetch)
                .MapO(DeserializeStringToObject);

        public EitherAsync<CacheFailure, Unit> Set(string cacheKey, T item)
            => SerializeObjectToString(item)
                .Bind(SetBytes(jsonStr => _cache.SetStringAsync(cacheKey, jsonStr)));

        private static EitherAsync<CacheFailure, string> SerializeObjectToString(T item) =>
                Try(() => JsonSerializer.Serialize(item))
                    .ToEitherAsync()
                    .MapLeft(CacheFailureCon.Serialization);

        private static EitherAsync<CacheFailure, T> DeserializeStringToObject(string jsonString) =>
            Try(() => JsonSerializer.Deserialize<T>(jsonString) ?? throw new Exception("Deserialized object is null"))
                .ToEitherAsync()
                .MapLeft(CacheFailureCon.Deserialization);

        private static Func<string, EitherAsync<CacheFailure, Unit>> SetBytes(Func<string, Task> cacheSet) =>
            jsonString =>
            TryAsync(() => cacheSet(jsonString).ToUnit())
                .ToEither()
                .MapLeft(CacheFailureCon.Insert);
    }
}