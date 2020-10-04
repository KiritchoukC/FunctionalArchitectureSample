using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Architecture.DataSource.MongoDb.Common.Cache;
using Architecture.Domain.Todo;
using LanguageExt;
using Microsoft.Extensions.Caching.Distributed;
using static LanguageExt.Prelude;

namespace Architecture.DataSource.MongoDb.Todo
{
    public class TodoItemDataSource : ITodoItemDataSource
    {
        private const string CacheKey = "TodoItemCacheKey";
        private readonly IDistributedCache _cache;

        public TodoItemDataSource(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<Either<TodoFailure, Option<IEnumerable<TodoItem>>>> GetAll(CancellationToken token)
        {
            return await CacheHelper.Get(() => _cache.GetAsync(CacheKey, token))
                .BindT(CacheHelper.Decode)
                .BindT(CacheHelper.Deserialize<IEnumerable<TodoItem>>);
        }

        public async Task<Either<TodoFailure, TodoItem>> Get(Guid id, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task<Either<TodoFailure, Guid>> Add(TodoItem todoItem, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task<Either<TodoFailure, Unit>> Update(TodoItem todoItem, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}