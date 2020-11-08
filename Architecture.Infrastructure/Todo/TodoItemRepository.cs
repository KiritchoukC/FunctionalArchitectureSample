namespace Architecture.Infrastructure.Todo
{
    using System.Threading.Tasks;

    using Architecture.DataSource.Cache;
    using Architecture.DataSource.MongoDb.Todo;
    using Architecture.Domain.Todo;

    using LanguageExt;

    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Logging;

    using static LanguageExt.Prelude;
    using Architecture.Utils.Functions;

    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly ITodoItemDataSource _todoItemDataSource;
        private readonly ICache<Seq<TodoItem>> _cache;

        public TodoItemRepository(
            IDistributedCache cache,
            ITodoItemDataSource todoItemDataSource,
            ILogger logger)
        {
            _cache = new RedisCache<Seq<TodoItem>>("TodoItemsCacheKey", cache, logger);
            _todoItemDataSource = todoItemDataSource;
        }

        public EitherAsync<TodoFailure, Seq<TodoItem>> GetAllAsync() =>
            RetrieveCacheAsync()
                .Bind(cacheOpt =>
                    cacheOpt.Match(
                        xs => Right(xs).ToAsync(),
                        () => RetrieveAndCache()));

        public EitherAsync<TodoFailure, Option<TodoItem>> GetByIdAsync(TodoId id) =>
            GetAllAsync()
                .Map(xs => xs.Find(x => x.Id == id));

        public EitherAsync<TodoFailure, Unit> AddAsync(TodoItem item) =>
            from cache in GetAllAsync()
            from updatedCache in Right(cache.Add(item)).ToAsync()
            from _1 in UpdateCache(updatedCache)
            from _2 in PersistAsync(item)
            select _2;

        private EitherAsync<TodoFailure, Seq<TodoItem>> RetrieveAndCache()
        {
            var dbResults = RetrieveAsync();

            return dbResults.Bind(UpdateCache)
                .Bind(_ => dbResults);
        }

        private EitherAsync<TodoFailure, Unit> PersistAsync(TodoItem item) =>
            TodoItemTranslator.ToDto(item).Apply(_todoItemDataSource.AddAsync)
                .MapLeft(TodoFailureCon.Database);

        private EitherAsync<TodoFailure, Unit> UpdateCache(Seq<TodoItem> items) =>
            _cache.Set(items)
                .MapLeft(TodoFailureCon.Cache).ToAsync();

        private EitherAsync<TodoFailure, Seq<TodoItem>> RetrieveAsync() =>
            _todoItemDataSource.GetAllAsync()
                .MapLeft(TodoFailureCon.Database)
                .MapT(TodoItemTranslator.FromDto);

        private EitherAsync<TodoFailure, Option<Seq<TodoItem>>> RetrieveCacheAsync() =>
            _cache.GetAsync()
                .MapLeft(TodoFailureCon.Cache);

        private Either<TodoFailure, T> Right<T>(T value) => GenericFunctions.Right<TodoFailure, T>(value);
    }
}
