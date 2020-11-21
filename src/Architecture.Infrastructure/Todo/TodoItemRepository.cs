namespace Architecture.Infrastructure.Todo
{
    using System.Collections.Generic;
    using System.Linq;

    using Architecture.DataSource.Cache;
    using Architecture.DataSource.MongoDb.Todo;
    using Architecture.Domain.Todo;
    using Architecture.Utils.Extensions;

    using LanguageExt;

    using static Architecture.Utils.Constructors.Constructors;
    using static LanguageExt.Prelude;

    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly ITodoItemDataSource _todoItemDataSource;
        private readonly ICache<List<TodoItemDto>> _cache;
        private const string _cacheKey = "TodoItemCacheKey";

        public TodoItemRepository(
            ICache<List<TodoItemDto>> cache,
            ITodoItemDataSource todoItemDataSource)
        {
            _cache = cache;
            _todoItemDataSource = todoItemDataSource;
        }

        public EitherAsync<TodoFailure, Seq<TodoItem>> GetAll() =>
            RetrieveCache()
                .Bind(cacheOpt =>
                    cacheOpt.Match(
                        xs => xs.ToEitherAsync(),
                        () => RetrieveAndCache()));

        public EitherAsync<TodoFailure, Option<TodoItem>> GetById(TodoId id) =>
            GetAll()
                .Map(xs => xs.Find(x => x.Id == id));

        public EitherAsync<TodoFailure, Unit> Add(TodoItem item) =>
            from cache in GetAll()
            from updatedCache in cache.Add(item).ToEitherAsync()
            from _1 in UpdateCache(updatedCache)
            from _2 in PersistAsync(item)
            select _2;

        private EitherAsync<TodoFailure, Seq<TodoItem>> RetrieveAndCache() =>
            from db in Retrieve()
            from _ in UpdateCache(db)
            select db;

        private EitherAsync<TodoFailure, Unit> PersistAsync(TodoItem item) =>
            TodoItemTranslator.ToDto(item).Apply(_todoItemDataSource.AddAsync)
                .MapLeft(TodoFailureCon.Database);

        private EitherAsync<TodoFailure, Unit> UpdateCache(Seq<TodoItem> items) =>
            items.Select(TodoItemTranslator.ToDto)
                .Apply(xs => _cache.SetAsync(_cacheKey, xs.ToList()))
                .MapLeft(TodoFailureCon.Cache);

        private EitherAsync<TodoFailure, Seq<TodoItem>> Retrieve() =>
            _todoItemDataSource.GetAllAsync()
                .MapLeft(TodoFailureCon.Database)
                .Bind(Translate);

        private EitherAsync<TodoFailure, Option<Seq<TodoItem>>> RetrieveCache() =>
            _cache.GetAsync(_cacheKey)
                .MapLeft(TodoFailureCon.Cache)
                .MapO(items => Translate(new Seq<TodoItemDto>(items)));

        private EitherAsync<TodoFailure, Seq<TodoItem>> Translate(Seq<TodoItemDto> dtos) =>
            dtos
                .Select(TodoItemTranslator.FromDto)
                .Sequence()
                .ToEither()
                .ToAsync()
                .MapLeft(TodoFailureCon.Translation);
    }

    public static class TodoItemRepositoryX
    {
        public static Either<TodoFailure, TRight> ToEither<TRight>(this TRight @this) => Right(@this);
        public static EitherAsync<TodoFailure, TRight> ToEitherAsync<TRight>(this TRight @this) => @this.ToEither().ToAsync();
    }
}
