using System;
using Architecture.Domain.Common.Cache;
using Architecture.Domain.Common.Database;
using Architecture.Domain.Todo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Architecture.Presentation.Common
{
    public static class FailuresHandlers
    {
        public static string HandleFailure(TodoFailure failure)
            => failure switch
            {
                Cache cache => HandleCacheFailure(cache.CacheFailure),
                Database database => HandleDatabaseFailure(database.DatabaseFailure),
                _ => throw new ArgumentOutOfRangeException(nameof(failure))
            };

        public static string HandleCacheFailure(CacheFailure failure)
            => failure switch
            {
                Decoding _        => "An error occured with the cache decoding",
                Deserialization _ => "An error occured with the cache deserialization",
                Encoding _        => "An error occured with the cache encoding",
                Insert _          => "An error occured while inserting item in cache",
                Fetch _           => "An error occured while getting item from cache",
                Serialization _   => "An error occured with the cache serialization",
                _ => throw new ArgumentOutOfRangeException(nameof(failure))
            };

        public static string HandleDatabaseFailure(DatabaseFailure failure)
            => failure switch
            {
                Retrieve retrieve => retrieve.Ex.ToString(),
                _ => throw new ArgumentOutOfRangeException(nameof(failure))
            };
    }
}