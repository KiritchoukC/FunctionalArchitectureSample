
namespace Architecture.DataSource.Cache
{
    using System;
    using System.Threading.Tasks;
    using Architecture.Domain.Common.Cache;
    using LanguageExt;
    using static LanguageExt.Prelude;
    using static Architecture.Utils.Constructors.Constructors;
    using System.Text.Json;

    public static class CacheHelper
    {
        /// <summary>
        /// Get item from cache as bytes asynchronously
        /// </summary>
        /// <param name="cacheGet">The asynchronous function getting the bytes from cache</param>
        /// <returns>
        /// - A cache failure
        /// - None if item does not exist in cache
        /// - Some bytes if item exist in cache
        /// </returns>
        public static EitherAsync<CacheFailure, Option<T>> GetBytes<T>(Func<Task<T>> cacheGetAsync) =>
            TryAsync(async () => (await cacheGetAsync()).Apply(Optional))
                .ToEither()
                .MapLeft(_ => CacheFailureCon.Fetch());

        /// <summary>
        /// Set item to cache as bytes asynchronously
        /// </summary>
        /// <param name="cacheSet">The asynchronous function setting the bytes to cache</param>
        /// <returns>
        /// - A cache insert failure
        /// - None
        /// - Some bytes if item exist in cache
        /// </returns>
        public static EitherAsync<CacheFailure, Unit> SetBytes(Func<Task> cacheSet) =>
            TryAsync(async () => { await cacheSet(); return unit; })
                .ToEither()
                .MapLeft(_ => CacheFailureCon.Insert());

        /// <summary>
        /// Deserialize the json string to type T
        /// </summary>
        /// <param name="jsonString">The json string to deserialize</param>
        /// <typeparam name="T">The type to deserialize to</typeparam>
        /// <returns>
        /// - A deserialization failure
        /// - None if json string is null
        /// - Some T object if json string is valid
        /// </returns>
        public static EitherAsync<CacheFailure, T> DeserializeStringToObject<T>(string jsonString) =>
            Try(() => JsonSerializer.Deserialize<T>(jsonString) ?? throw new Exception("Deserialized object is null"))
                .ToEitherAsync()
                .MapLeft(_ => CacheFailureCon.Deserialization());

        /// <summary>
        /// Serialize an object to a json string
        /// </summary>
        /// <param name="item">The item serialize</param>
        /// <typeparam name="T">The item's type</typeparam>
        /// <returns>
        /// - A serialization failure
        /// - None if item is null
        /// - Some json string
        /// </returns>
        public static EitherAsync<CacheFailure, string> SerializeObjectToString<T>(T item) =>
                Try(() => JsonSerializer.Serialize(item))
                    .ToEither()
                    .MapLeft(_ => CacheFailureCon.Serialization())
                    .ToAsync();
    }
}