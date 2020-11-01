namespace Architecture.Infrastructure.Todo
{
    using System.Threading.Tasks;

    using Architecture.DataSource.Cache;
    using Architecture.DataSource.MongoDb.Todo;
    using Architecture.Domain.Todo;

    using LanguageExt;

    using Microsoft.Extensions.Caching.Distributed;

    using static LanguageExt.Prelude;

    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly ITodoItemDataSource _todoItemDataSource;
        private readonly ICache<Seq<TodoItem>> _cache;

        public TodoItemRepository(
            IDistributedCache cache,
            ITodoItemDataSource todoItemDataSource)
        {
            _cache = new RedisCache<Seq<TodoItem>>("TodoItemsCacheKey", cache);
            _todoItemDataSource = todoItemDataSource;
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

        public async Task<Either<TodoFailure, Option<TodoItem>>> GetByIdAsync(TodoId id)
        {
            var getFromDatabaseIfCacheIsNone =
                fun(
                    (Option<Seq<TodoItem>> cacheItems) =>
                        cacheItems.Map(items => items.Find(x => x.Id == id))
                            .Flatten()
                            .Match(
                                item => Right(Optional(item)),
                                () =>
                                    RetrieveFromDatabaseAndFillCache()
                                        .Map(items => items.Find(x => x.Id == id)))
                );

            return (await GetFromCacheAsync())
                .Bind(getFromDatabaseIfCacheIsNone);
        }

        public async Task<Either<TodoFailure, Unit>> AddAsync(TodoItem item)
        {
            var addItemToSeq = fun(
                (Option<Seq<TodoItem>> items, TodoItem i) =>
                    items.Match(
                        xs => xs.Add(i),
                        () => Seq<TodoItem>(i))
            );

            var addToCacheAsync = fun(
                async (TodoItem i) => (await _cache.GetAsync())
                    .Bind<Seq<TodoItem>>(xs => Right(addItemToSeq(xs, item)))
                    .Map(items => _cache.Set(items))
                    .Flatten()
                    .MapLeft(TodoFailureCon.Cache)
            );

            var persist = fun(
                (TodoItem todo) =>
                    TodoItemTranslator.ToDto(todo)
                    .Apply(_todoItemDataSource.Add)
                    .MapLeft(TodoFailureCon.Database));

            return await persist(item).Apply(asTask)
                .BindT(_ => addToCacheAsync(item));
        }

        private Either<TodoFailure, Seq<TodoItem>> RetrieveFromDatabaseAndFillCache() =>
            from dbResult in GetFromDatabase()
            from cacheResult in UpdateCache(dbResult)
            select cacheResult;

        private Either<TodoFailure, Seq<TodoItem>> UpdateCache(Seq<TodoItem> items)
        {
            return _cache.Set(items)
                .Map(_ => items)
                .MapLeft(TodoFailureCon.Cache);
        }

        private Either<TodoFailure, Seq<TodoItem>> GetFromDatabase()
            => _todoItemDataSource.GetAll()
                .MapLeft(TodoFailureCon.Database)
                .MapT(TodoItemTranslator.FromDto);

        private async Task<Either<TodoFailure, Option<Seq<TodoItem>>>> GetFromCacheAsync()
            => (await _cache.GetAsync())
                .MapLeft(TodoFailureCon.Cache);
    }
}
