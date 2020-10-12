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
        private readonly ICache _cache;

        public TodoItemRepository(
            IDistributedCache cache,
            ITodoItemDataSource todoItemDataSource)
        {
            _cache = new RedisCache("TodoItemsCacheKey", cache);
            _todoItemDataSource = todoItemDataSource;
        }

        public Either<TodoFailure, IEnumerable<TodoItem>> GetAll()
        {
            var updateCache = fun<IEnumerable<TodoItem>, Either<TodoFailure, IEnumerable<TodoItem>>>(
                items =>
                    items.ToList()
                        .Apply(_cache.Set)
                        .Map(x => items)
                        .MapLeft(TodoFailureCon.Cache));

            var getFromDatabaseIfCacheIsNone = fun<Option<IEnumerable<TodoItem>>, Either<TodoFailure, IEnumerable<TodoItem>>>()
            
            Either<TodoFailure, IEnumerable<TodoItem>> GetFromDatabaseIfCacheIsNone(
                Option<IEnumerable<TodoItem>> cacheItems)
            {
                return cacheItems.Match<Either<TodoFailure, IEnumerable<TodoItem>>>(
                    x => Right(x),
                    () =>
                    {
                        return databaseResult
                            .Bind(updateCache);
                    });
            }

            var databaseResult = _todoItemDataSource.GetAll()
                .MapLeft(ex => TodoFailureCon.Database(DatabaseFailureCon.Retrieve(ex)))
                .MapT(TodoItemTranslator.FromDto);

            return _cache.Get<IEnumerable<TodoItem>>()
                .MapLeft(TodoFailureCon.Cache)
                .Bind(GetFromDatabaseIfCacheIsNone);
        }

        public Either<TodoFailure, Option<TodoItem>> GetById(Guid id)
        {
            return _cache.Get<IEnumerable<TodoItem>>()
                .MapLeft(TodoFailureCon.Cache)
                .MapT(items => items.SingleOrDefault(x => x.Id == id))
                .BindT(Optional);
        }

        public Either<TodoFailure, Unit> Add(TodoItem item)
        {
            throw new NotImplementedException();
        }
    }
}