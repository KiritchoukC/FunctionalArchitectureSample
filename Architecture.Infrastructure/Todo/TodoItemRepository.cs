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
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly ITodoItemDataSource _todoItemDataSource;
        private readonly ICache<IEnumerable<TodoItem>> _cache;

        public TodoItemRepository(
            IDistributedCache cache,
            ITodoItemDataSource todoItemDataSource)
        {
            _cache = new RedisCache<IEnumerable<TodoItem>>("TodoItemsCacheKey", cache);
            _todoItemDataSource = todoItemDataSource;
        }

        public Either<TodoFailure, IEnumerable<TodoItem>> GetAll()
        {
            var getFromDatabaseIfCacheIsNone =
                fun(
                    (Option<IEnumerable<TodoItem>> cacheItems) =>
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
                    (Option<IEnumerable<TodoItem>> cacheItems) =>
                        cacheItems.Map(items => items.Find(x => x.Id == id))
                            .Flatten()
                            .Match(
                                item => Right(Optional(item)),
                                () => RetrieveFromDatabaseAndFillCache()
                                    .Map(items => items.Find(x => x.Id == id)))
                );

            return GetFromCache()
                .Bind(getFromDatabaseIfCacheIsNone);
        }

        public Either<TodoFailure, Unit> Add(TodoItem item)
        {
            var addItemToListIfSome = fun(
                (Option<IEnumerable<TodoItem>> items, TodoItem i) =>
                    items.Match(
                        xs => xs.Prepend(i),
                        () => (new[] {i}).AsEnumerable())
            );

            var addToCache = fun(
                (TodoItem i) => _cache.Get()
                    .Bind<IEnumerable<TodoItem>>(x => Right(addItemToListIfSome(x, item)))
                    .Map(items => _cache.Set(items))
                    .Flatten()
                    .MapLeft(TodoFailureCon.Cache)
            );

            return TodoItemTranslator.ToDto(item)
                .Apply(_todoItemDataSource.Add)
                .MapLeft(TodoFailureCon.Database)
                .Bind(_ => addToCache(item));
        }

        private Either<TodoFailure, IEnumerable<TodoItem>> RetrieveFromDatabaseAndFillCache()
        {
            return GetFromDatabase()
                .Bind(UpdateCache);
        }

        private Either<TodoFailure, IEnumerable<TodoItem>> UpdateCache(IEnumerable<TodoItem> items)
        {
            var lst = items.ToList();
            return _cache.Set(lst)
                .Map(_ => lst.AsEnumerable())
                .MapLeft(TodoFailureCon.Cache);
        }

        private Either<TodoFailure, IEnumerable<TodoItem>> GetFromDatabase()
            => _todoItemDataSource.GetAll()
                .MapLeft(TodoFailureCon.Database)
                .MapT(TodoItemTranslator.FromDto);

        private Either<TodoFailure, Option<IEnumerable<TodoItem>>> GetFromCache()
            => _cache.Get().MapLeft(TodoFailureCon.Cache);
    }
}