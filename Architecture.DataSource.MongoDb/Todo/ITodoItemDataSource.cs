
using System.Threading;
using System.Threading.Tasks;
using LanguageExt;

namespace Architecture.DataSource.MongoDb.Todo
{
    using System;
    using System.Collections.Generic;
    using Architecture.Domain.Todo;

    public interface ITodoItemDataSource
    {
        Task<Either<TodoFailure, IEnumerable<TodoItem>>> GetAll(CancellationToken token);
        Task<Either<TodoFailure, TodoItem>> Get(Guid id, CancellationToken token);
        Task<Either<TodoFailure, Guid>> Add(TodoItem todoItem, CancellationToken token);
        Task<Either<TodoFailure, Unit>> Update(TodoItem todoItem, CancellationToken token);
    }
}