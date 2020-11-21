namespace Architecture.DataSource.Cache
{
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Architecture.Domain.Common.Cache;
    using Architecture.Utils.Extensions;

    using LanguageExt;

    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Logging;

    using static Architecture.Utils.Constructors.Constructors;
    using static LanguageExt.Prelude;

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
            TryAsync(async () => (await _cache.GetStringAsync(_cacheKey)).Apply(Optional))
                .ToEither()
                .MapLeft(CacheFailureCon.Fetch)
                .MapO(DeserializeStringToObject);

        public EitherAsync<CacheFailure, Unit> SetAsync(T item)
            => SerializeObjectToString(item)
                .Bind(SetBytes(jsonStr => _cache.SetStringAsync(_cacheKey, jsonStr)));

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