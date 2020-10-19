using System;
using System.Text;
using Architecture.Domain.Common.Cache;
using LanguageExt;
using static LanguageExt.Prelude;
using static Newtonsoft.Json.JsonConvert;
using Encoding = System.Text.Encoding;

namespace Architecture.DataSource.Cache
{
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
        public static Either<CacheFailure, Option<byte[]>> GetBytes(Func<Option<byte[]>> cacheGet)
        {
            return Try(cacheGet)
                .ToEither()
                .MapLeft(e => CacheFailureCon.Fetch());
        }

        /// <summary>
        /// Set item to cache as bytes asynchronously
        /// </summary>
        /// <param name="cacheSet">The asynchronous function setting the bytes to cache</param>
        /// <returns>
        /// - A cache insert failure
        /// - None
        /// - Some bytes if item exist in cache
        /// </returns>
        public static Either<CacheFailure, Unit> SetBytes(Action cacheSet)
        {
            return Try(() =>
                {
                    cacheSet();
                    return unit;
                })
                .ToEither()
                .MapLeft(
                    e => CacheFailureCon.Insert());
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
        public static Either<CacheFailure, Option<string>> DecodeBytesToString(byte[] bytes)
        {
            return
                TryOption(() => Encoding.UTF8.GetString(bytes))
                    .ToEither()
                    .MapLeft(_ => CacheFailureCon.Decoding());
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
        public static Either<CacheFailure, Option<T>> DeserializeStringToObject<T>(string jsonString)
        {
            return
                TryOption(() => DeserializeObject<T>(jsonString))
                    .ToEither()
                    .MapLeft(_ => CacheFailureCon.Deserialization());
        }

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
        public static Either<CacheFailure, string> SerializeObjectToString<T>(T item)
        {
            return
                Try(() => SerializeObject(item))
                    .ToEither()
                    .MapLeft(
                        _ => CacheFailureCon.Serialization());
        }

        /// <summary>
        /// Encode the string to a bytes array
        /// </summary>
        /// <param name="jsonStr">The json string to encode</param>
        /// <returns>
        /// - A encoding failure
        /// - None
        /// - Some bytes array
        /// </returns>
        public static Either<CacheFailure, byte[]> EncodeStringToBytes(string jsonStr)
        {
            return
                Try(() => Encoding.UTF8.GetBytes(jsonStr))
                    .ToEither()
                    .MapLeft(
                        e => CacheFailureCon.Encoding());
        }
    }
}