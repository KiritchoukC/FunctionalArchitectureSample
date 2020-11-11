
namespace Architecture.Domain.Todo
{
    using Architecture.Domain.Common.Cache;
    using Architecture.Domain.Common.Database;

    using OneOf;

    public abstract class TodoFailure : OneOfBase<
        TodoFailure.Cache,
        TodoFailure.Database>
    {
        public class Cache : TodoFailure
        {
            public readonly CacheFailure Failure;
            public Cache(CacheFailure failure) => Failure = failure;
        }
        public class Database : TodoFailure
        {
            public readonly DatabaseFailure Failure;
            public Database(DatabaseFailure failure) => Failure = failure;
        }
    }
}