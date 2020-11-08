namespace Architecture.Infrastructure.Todo
{
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using Architecture.DataSource.Cache;
    using Architecture.DataSource.MongoDb.Todo;
    using Architecture.Domain.Todo;
    using Architecture.Utils.Extensions;
    using LanguageExt;

    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Logging;
    using static LanguageExt.Prelude;

    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly ITodoItemDataSource _todoItemDataSource;
        private readonly ILogger _logger;
        private readonly ICache<Seq<TodoItem>> _cache;

        public TodoItemRepository(
            IDistributedCache cache,
            ITodoItemDataSource todoItemDataSource,
            ILogger logger)
        {
            _cache = new RedisCache<Seq<TodoItem>>("TodoItemsCacheKey", cache, logger);
            _todoItemDataSource = todoItemDataSource;
            _logger = logger;
        }

        public async Task<Either<TodoFailure, Seq<TodoItem>>> GetAllAsync()
        {
            var getFromDatabaseIfCacheIsNone =
                fun(
                    (Option<Seq<TodoItem>> cacheItems) =>
                        cacheItems.Match(
                            items => Right(items),
                            RetrieveFromDatabaseAndFillCache)
                );

            return (await GetFromCacheAsync())
                .Bind(getFromDatabaseIfCacheIsNone);
        }

        public async Task<Either<TodoFailure, Option<TodoItem>>> GetByIdAsync(TodoId id) =>
            await GetAllAsync().MapT(xs => xs.Find(x => x.Id == id));

        public async Task<Either<TodoFailure, Unit>> AddAsync(TodoItem item) =>
                from cache in await GetAllAsync()
                from _1 in PersistToDatabase(item)
                from updatedCache in Right(cache.Add(item))
                from _2 in UpdateCache(updatedCache)
                select _2;

        private Either<TodoFailure, Seq<TodoItem>> RetrieveFromDatabaseAndFillCache() =>
            from dbResult in GetFromDatabase()
            from _ in UpdateCache(dbResult)
            select dbResult;

        private Either<TodoFailure, Unit> PersistToDatabase(TodoItem item) =>
            TodoItemTranslator.ToDto(item)
            .Apply(_todoItemDataSource.Add)
            .MapLeft(TodoFailureCon.Database);

        private Either<TodoFailure, Unit> UpdateCache(Seq<TodoItem> items) =>
            _cache.Set(items)
                .MapLeft(TodoFailureCon.Cache);

        private Either<TodoFailure, Seq<TodoItem>> GetFromDatabase() =>
            _todoItemDataSource.GetAll()
                .MapLeft(TodoFailureCon.Database)
                .MapT(TodoItemTranslator.FromDto);

        private async Task<Either<TodoFailure, Option<Seq<TodoItem>>>> GetFromCacheAsync() =>
            (await _cache.GetAsync())
                .MapLeft(TodoFailureCon.Cache);
    }
}
