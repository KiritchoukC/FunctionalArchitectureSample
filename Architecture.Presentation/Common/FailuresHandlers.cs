
namespace Architecture.Presentation.Common
{
    using Architecture.Domain.Common.Cache;
    using Architecture.Domain.Common.Database;
    using Architecture.Domain.Todo;

    public static class FailuresHandlers
    {
        public static string HandleFailure(TodoFailure failure)
            => failure.Match(
                cacheFailure => HandleCacheFailure(cacheFailure.Failure),
                databaseFailure => HandleDatabaseFailure(databaseFailure.Failure),
                validationFailure => validationFailure.ErrorsJoined(),
                translationFailure => translationFailure.ErrorsJoined());

        public static string HandleCacheFailure(CacheFailure failure)
            => failure.Match(
                fetchFailure => "An error occured with the cache decoding",
                insertFailure => "An error occured with the cache deserialization",
                serializationFailure => "An error occured while getting item from cache",
                deserializationFailure => "An error occured with the cache serialization");

        public static string HandleDatabaseFailure(DatabaseFailure failure)
            => failure.Match(
                retrieveFailure => retrieveFailure.Error.ToString());
    }
}