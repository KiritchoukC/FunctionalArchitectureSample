using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Architecture.DataSource.Cache;
using Architecture.Domain.Todo;
using LanguageExt;
using LanguageExt.SomeHelp;
using Microsoft.Extensions.Caching.Distributed;
using static LanguageExt.Prelude;

namespace Architecture.DataSource.MongoDb.Todo
{
    public class TodoItemDataSource : ITodoItemDataSource
    {
        private readonly ICache _cache;

        public TodoItemDataSource(IDistributedCache cache)
        {
            _cache = new RedisCache("TodoItemsCacheKey", cache);
        }

        public async Task<Either<TodoFailure, Option<IEnumerable<TodoItem>>>> GetAll(CancellationToken token)
        {
            return (await _cache.GetAsync<IEnumerable<TodoItem>>(token))
                .MapLeft(TodoFailures.Cache);
        }

        public async Task<Either<TodoFailure, Option<TodoItem>>> GetById(Guid id, CancellationToken token)
        {
            return (await _cache.GetAsync<IEnumerable<TodoItem>>(token))
                .MapLeft(TodoFailures.Cache)
                .MapT(items => items.SingleOrDefault(x => x.Id == id))
                .BindT(Optional);
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