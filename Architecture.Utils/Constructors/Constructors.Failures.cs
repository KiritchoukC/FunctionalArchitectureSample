
namespace Architecture.Utils.Constructors
{
    using Architecture.Domain.Common.Cache;
    using Architecture.Domain.Common.Database;
    using Architecture.Domain.Todo;

    using LanguageExt.Common;

    public static partial class Constructors
    {
        public static class CacheFailureCon
        {
            public static CacheFailure Fetch() => new CacheFailure.Fetch();
            public static CacheFailure Insert() => new CacheFailure.Insert();
            public static CacheFailure Decoding() => new CacheFailure.Decoding();
            public static CacheFailure Encoding() => new CacheFailure.Encoding();
            public static CacheFailure Serialization() => new CacheFailure.Serialization();
            public static CacheFailure Deserialization() => new CacheFailure.Deserialization();
        }
        public static class DatabaseFailureCon
        {
            public static DatabaseFailure Retrieve(Error error) => new DatabaseFailure.Retrieve(error);
        }
        public static class TodoFailureCon
        {
            public static TodoFailure Database(DatabaseFailure failure) => new TodoFailure.Database(failure);
            public static TodoFailure Cache(CacheFailure failure) => new TodoFailure.Cache(failure);
        }
    }
}
