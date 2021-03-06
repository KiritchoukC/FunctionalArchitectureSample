﻿namespace Architecture.Infrastructure.Todo
{
    using System.Collections.Generic;
    using System.Linq;

    using Architecture.Domain.Todo;
    using Architecture.Utils;

    using LanguageExt;

    using static Architecture.Utils.Constructors;
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
            GetAll().Map(xs => xs.Find(x => x.Id == id));

        public EitherAsync<TodoFailure, Unit> Add(TodoItem item) =>
            from items in GetAll()
            from updatedCache in items.Add(item).ToEitherAsync()
            from _1 in UpdateCache(updatedCache)
            from _2 in PersistAsync(item)
            select _2;

        private EitherAsync<TodoFailure, Seq<TodoItem>> RetrieveAndCache() =>
            from db in Retrieve()
            from _ in UpdateCache(db)
            select db;

        private EitherAsync<TodoFailure, Unit> PersistAsync(TodoItem item) =>
            TodoItemDto.FromDomain(item).Apply(_todoItemDataSource.Add)
                .MapLeft(TodoFailureCon.Database);

        private EitherAsync<TodoFailure, Unit> UpdateCache(Seq<TodoItem> items) =>
            items.Select(TodoItemDto.FromDomain)
                .Apply(xs => _cache.Set(_cacheKey, xs.ToList()))
                .MapLeft(TodoFailureCon.Cache);

        private EitherAsync<TodoFailure, Seq<TodoItem>> Retrieve() =>
            _todoItemDataSource.GetAll()
                .MapLeft(TodoFailureCon.Database)
                .Bind(Translate);

        private EitherAsync<TodoFailure, Option<Seq<TodoItem>>> RetrieveCache() =>
            _cache.Get(_cacheKey)
                .MapLeft(TodoFailureCon.Cache)
                .MapO(items => Translate(Seq(items)));

        private EitherAsync<TodoFailure, Seq<TodoItem>> Translate(Seq<TodoItemDto> dtos) =>
            dtos
                .Select(TodoItemDto.ToDomain)
                .Sequence()
                .ToEither()
                .ToAsync()
                .MapLeft(TodoFailureCon.Translation);
    }

    internal static class TodoItemRepositoryX
    {
        public static Either<TodoFailure, TRight> ToEither<TRight>(this TRight @this) => Right(@this);
        public static EitherAsync<TodoFailure, TRight> ToEitherAsync<TRight>(this TRight @this) => @this.ToEither().ToAsync();
    }
}
