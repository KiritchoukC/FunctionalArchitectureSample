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
    using static Architecture.Utils.Constructors.Constructors;
    using Architecture.Utils.Functions;
    using System.Collections.Generic;
    using Architecture.Utils.Extensions;
    using Architecture.Domain.Common.Cache;

    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly ITodoItemDataSource _todoItemDataSource;
        private readonly ICache<List<TodoItem>> _cache;

        public TodoItemRepository(
            IDistributedCache cache,
            ITodoItemDataSource todoItemDataSource,
            ILogger logger)
        {
            _cache = new RedisCache<List<TodoItem>>("TodoItemsCacheKey", cache, logger);
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

        public EitherAsync<TodoFailure, Unit> AddAsync(TodoItem item) => unit;
            //from cache in GetAllAsync()
            //from updatedCache in Right(cache.Add(item)).ToAsync()
            //from _1 in UpdateCache(updatedCache)
            //from _2 in PersistAsync(item)
            //select _2;

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
            _cache.SetAsync(items.ToList())
                .MapLeft(TodoFailureCon.Cache);

        private EitherAsync<TodoFailure, Seq<TodoItem>> RetrieveAsync() =>
            _todoItemDataSource.GetAllAsync()
                .MapLeft(TodoFailureCon.Database)
                .Bind(Translate);

        private EitherAsync<TodoFailure, Option<Seq<TodoItem>>> RetrieveCacheAsync() =>
            _cache.GetAsync()
                .MapLeft(TodoFailureCon.Cache)
                .MapO<TodoFailure, List<TodoItem>, Seq<TodoItem>>(items => new Seq<TodoItem>(items));

        private static Either<TodoFailure, T> Right<T>(T value) => GenericFunctions.Right<TodoFailure, T>(value);

        private EitherAsync<TodoFailure, Seq<TodoItem>> Translate(Seq<TodoItemDto> dtos) =>
            dtos
                .Select(TodoItemTranslator.FromDto)
                .Sequence()
                .ToEither()
                .ToAsync()
                .MapLeft(TodoFailureCon.Translation);
    }
}
