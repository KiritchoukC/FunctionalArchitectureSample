using Architecture.Domain.Common.Cache;
using Architecture.Domain.Common.Database;
using LanguageExt;

namespace Architecture.Domain.Todo
{
    [Union]
    public abstract partial class TodoFailure
    {
        public abstract TodoFailure Cache(CacheFailure cacheFailure);
        public abstract TodoFailure Database(DatabaseFailure databaseFailure);
    }
}