
namespace Architecture.Utils.Constructors
{
    using Architecture.Domain.Common.Cache;
    using Architecture.Domain.Common.Database;
    using Architecture.Domain.Todo;

    using LanguageExt;
    using LanguageExt.Common;

    public static partial class Constructors
    {
        public static class CacheFailureCon
        {
            public static CacheFailure Fetch(Error error) => new CacheFailure.Fetch(error);
            public static CacheFailure Insert(Error error) => new CacheFailure.Insert(error);
            public static CacheFailure Serialization(Error error) => new CacheFailure.Serialization(error);
            public static CacheFailure Deserialization(Error error) => new CacheFailure.Deserialization(error);
        }
        public static class DatabaseFailureCon
        {
            public static DatabaseFailure Retrieve(Error error) => new DatabaseFailure.Retrieve(error);
            public static DatabaseFailure Insert(Error error) => new DatabaseFailure.Insert(error);
            public static DatabaseFailure Update(Error error) => new DatabaseFailure.Update(error);
        }
        public static class TodoFailureCon
        {
            public static TodoFailure Database(DatabaseFailure failure) => new TodoFailure.Database(failure);
            public static TodoFailure Cache(CacheFailure failure) => new TodoFailure.Cache(failure);
            public static TodoFailure Validation(Seq<Error> errors) => new TodoFailure.Validation(errors);
            public static TodoFailure Translation(Seq<Error> errors) => new TodoFailure.Translation(errors);
        }
    }
}
