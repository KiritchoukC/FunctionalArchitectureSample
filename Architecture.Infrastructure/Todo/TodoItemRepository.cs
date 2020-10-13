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
            var getFromDatabase =
                fun(
                    () =>
                        _todoItemDataSource.GetAll()
                            .MapLeft(TodoFailureCon.Database)
                            .MapT(TodoItemTranslator.FromDto)
                );

            var updateCache =
                fun(
                    (List<TodoItem> items) =>
                        _cache.Set(items)
                            .Map(x=> items.AsEnumerable())
                            .MapLeft(TodoFailureCon.Cache)
                );

            var getFromDatabaseIfCacheIsNone =
                fun(
                    (Option<IEnumerable<TodoItem>> cacheItems) =>
                        cacheItems.Match(
                            items => Right(items),
                            () => getFromDatabase()
                                .Map(Enumerable.ToList)
                                .Bind(updateCache))
                );

            var getFromCache =
                fun(
                    () => _cache.Get().MapLeft(TodoFailureCon.Cache)
                );

            return getFromCache()
                .Bind(getFromDatabaseIfCacheIsNone);
        }

        public Either<TodoFailure, Option<TodoItem>> GetById(Guid id)
        {
            var getFromDatabase =
                fun(
                    () =>
                        _todoItemDataSource.GetAll()
                            .MapLeft(TodoFailureCon.Database)
                            .MapT(TodoItemTranslator.FromDto)
                );

            var updateCache =
                fun(
                    (List<TodoItem> items) =>
                        _cache.Set(items)
                            .Map(x=> items.AsEnumerable())
                            .MapLeft(TodoFailureCon.Cache)
                );

            var getFromDatabaseIfCacheIsNone =
                fun(
                    (Option<IEnumerable<TodoItem>> cacheItems) =>
                        cacheItems.Map(items => items.Find(x => x.Id.Value == id))
                            .Flatten()
                            .Match(
                                item => Right(Optional(item)),
                                () => getFromDatabase()
                                    .Map(Enumerable.ToList)
                                    .Bind(updateCache)
                                    .Map(items => items.Find(x=> x.Id.Value == id)))
                );

            var getFromCache =
                fun(
                    () => _cache.Get().MapLeft(TodoFailureCon.Cache)
                );

            return getFromCache()
                .Bind(getFromDatabaseIfCacheIsNone);
        }

        public Either<TodoFailure, Unit> Add(TodoItem item)
        {
            var addItemToListIfSome = fun(
                (Option<IEnumerable<TodoItem>> items, TodoItem i) =>
                    items.Match(
                        xs => xs.Prepend(i),
                        () => (new []{i}).AsEnumerable())
                );
            
            var addToCache = fun(
                (TodoItem i) => _cache.Get()
                    .Bind<IEnumerable<TodoItem>>(x => Right(addItemToListIfSome(x, item)))
                    .Map(items => _cache.Set(items))
                    .Flatten()
                    .MapLeft(TodoFailureCon.Cache)
                );

            return _todoItemDataSource.Add(item)
                .MapLeft(TodoFailureCon.Database)
                .Bind(_ => addToCache(item));
        }
    }
}