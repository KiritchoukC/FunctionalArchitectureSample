
using System.Threading;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.Common;

namespace Architecture.DataSource.MongoDb.Todo
{
    using System;
    using System.Collections.Generic;
    using Architecture.Domain.Todo;

    public interface ITodoItemDataSource
    {
        Either<Exception, IEnumerable<TodoItemDto>> GetAll();
        Either<TodoFailure, Option<TodoItem>> GetById(Guid id);
        Either<TodoFailure, Guid> Add(TodoItem todoItem);
        Either<TodoFailure, Unit> Update(TodoItem todoItem);
    }
}