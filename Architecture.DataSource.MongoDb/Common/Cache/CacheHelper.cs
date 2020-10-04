using System;
using System.Text;
using System.Threading.Tasks;
using Architecture.Domain.Todo;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Architecture.DataSource.MongoDb.Common.Cache
{
    public static class CacheHelper
    {
        /// <summary>
        /// Get item from cache as bytes asynchronously
        /// </summary>
        /// <param name="cacheGetAsync">The asynchronous function getting the bytes from cache</param>
        /// <returns>
        /// - A cache failure
        /// - None if item does not exist in cache
        /// - Some bytes if item exist in cache
        /// </returns>
        public static Task<Either<TodoFailure, Option<byte[]>>> Get(Func<Task<byte[]>> cacheGetAsync)
        {
            return TryAsync(cacheGetAsync)
                .ToEither()
                .BiMap(
                    bytes => bytes.IsNull() ? None : Some(bytes),
                    e => TodoFailures.Cache())
                .ToEither();
        }

        /// <summary>
        /// Decode the bytes array to a string
        /// </summary>
        /// <param name="bytes">The bytes to decode</param>
        /// <returns>
        /// - A decode failure
        /// - None if bytes array is null
        /// - Some string if bytes array is valid
        /// </returns>
        public static Either<TodoFailure, Option<string>> Decode(Option<byte[]> bytes)
        {
            static Either<TodoFailure, Option<string>> Decode(byte[] bs)
            {
                return
                    Try(() => Encoding.UTF8.GetString(bs))
                        .ToEither()
                        .BiMap(
                            str => str.IsNull() ? None : Some(str),
                            e => TodoFailures.CacheDecoding());
            }

            return bytes.Match(
                Decode,
                () => Right(Option<string>.None)
            );
        }
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
        public static Either<TodoFailure, Option<T>> Deserialize<T>(Option<string> jsonString)
        {
            static Either<TodoFailure, Option<T>> Deserialize(string json) =>
                Try(() => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json))
                    .ToEither()
                    .BiMap(
                        item => item.IsNull() || item.IsDefault() ? None : Some(item),
                        e => TodoFailures.CacheDeserialization());

            return jsonString.Match(
                Deserialize,
                () => Right(Option<T>.None)
            );
        }
    }
}