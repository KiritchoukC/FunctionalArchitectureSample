using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Architecture.DataSource.Cache;
using Architecture.DataSource.MongoDb.Todo;
using Architecture.Domain.Common.Database;
using Architecture.Domain.Todo;
using LanguageExt;
using LanguageExt.DataTypes.Serialisation;
using Microsoft.Extensions.Caching.Distributed;
using static LanguageExt.Prelude;

namespace Architecture.Infrastructure.Todo
{
    public class TodoItemRepository: ITodoItemRepository
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

        public Either<TodoFailure, Seq<TodoItem>> GetAll()
        {
            var getFromDatabaseIfCacheIsNone =
                fun(
                    (Option<Seq<TodoItem>> cacheItems) =>
                        cacheItems.Match(
                            items => Right(items),
                            RetrieveFromDatabaseAndFillCache)
                );

            return GetFromCache()
                .Bind(getFromDatabaseIfCacheIsNone);
        }

        public Either<TodoFailure, Option<TodoItem>> GetById(TodoId id)
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

            return GetFromCache()
                .Bind(getFromDatabaseIfCacheIsNone);
        }

        public Either<TodoFailure, Unit> Add(TodoItem item)
        {
            var addItemToListIfSome = fun(
                (Option<Seq<TodoItem>> items, TodoItem i) =>
                    items.Match(
                        xs => xs.Add(i),
                        () => new Seq<TodoItem>(new []{i}))
            );

            var addToCache = fun(
                (TodoItem i) => _cache.Get()
                    .Bind<Seq<TodoItem>>(x => Right(addItemToListIfSome(x, item)))
                    .Map(items => _cache.Set(items))
                    .Flatten()
                    .MapLeft(TodoFailureCon.Cache)
            );

            return TodoItemTranslator.ToDto(item)
                .Apply(_todoItemDataSource.Add)
                .MapLeft(TodoFailureCon.Database)
                .Bind(_ => addToCache(item));
        }

        private Either<TodoFailure, Seq<TodoItem>> RetrieveFromDatabaseAndFillCache()
        {
            return GetFromDatabase()
                .Bind(UpdateCache);
        }

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

        private Either<TodoFailure, Option<Seq<TodoItem>>> GetFromCache()
            => _cache.Get().MapLeft(TodoFailureCon.Cache);
    }
}