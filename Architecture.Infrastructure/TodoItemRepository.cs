using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Architecture.DataSource.Cache;
using Architecture.Domain.Todo;
using LanguageExt;
using Microsoft.Extensions.Caching.Distributed;
using static LanguageExt.Prelude;

namespace Architecture.Infrastructure
{
    public interface ITodoItemRepository
    {
        
    }
    
    public class TodoItemRepository : ITodoItemRepository
    {
        
        private readonly ICache _cache;

        public TodoItemRepository(IDistributedCache cache)
        {
            _cache = new RedisCache("TodoItemsCacheKey", cache);
        }
        public async Task<Either<TodoFailure, Option<IEnumerable<TodoItem>>>> GetAllAsync(CancellationToken token)
        {
            return (await _cache.GetAsync<IEnumerable<TodoItem>>(token))
                .MapLeft(TodoFailures.Cache);
        }

        public async Task<Either<TodoFailure, Option<TodoItem>>> GetByIdAsync(Guid id, CancellationToken token)
        {
            return (await _cache.GetAsync<IEnumerable<TodoItem>>(token))
                .MapLeft(TodoFailures.Cache)
                .MapT(items => items.SingleOrDefault(x => x.Id == id))
                .BindT(Optional);
        }

        public async Task<Either<TodoFailure, Unit>> AddAsync(TodoItem item, CancellationToken token)
        {
            
        }
    }
}