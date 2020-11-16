
namespace Architecture.Presentation.Common
{
    using System;

    using Architecture.Domain.Common.Cache;
    using Architecture.Domain.Common.Database;
    using Architecture.Domain.Todo;

    public static class FailuresHandlers
    {
        public static string HandleFailure(TodoFailure failure) =>
            failure switch
            {
                TodoFailure.Cache f => HandleCacheFailure(f.Failure),
                TodoFailure.Database f => HandleDatabaseFailure(f.Failure),
                TodoFailure.Validation f => f.ErrorsJoined,
                TodoFailure.Translation f => f.ErrorsJoined,
                _ => throw UnhandledFailureException(failure)
            };

        public static string HandleCacheFailure(CacheFailure failure) =>
            failure switch
            {
                CacheFailure.Fetch => "An error occured with the cache decoding",
                CacheFailure.Insert => "An error occured with the cache deserialization",
                CacheFailure.Serialization => "An error occured while getting item from cache",
                CacheFailure.Deserialization => "An error occured with the cache serialization",
                _ => throw UnhandledFailureException(failure)
            };

        public static string HandleDatabaseFailure(DatabaseFailure failure) =>
            failure switch
            {
                DatabaseFailure.Retrieve f => f.Error.ToString(),
                DatabaseFailure.Insert f => f.Error.ToString(),
                _ => throw UnhandledFailureException(failure)
            };

        private static Exception UnhandledFailureException<T>(T failure) => new($"Failure {failure?.GetType()} is not handled");
    }
}