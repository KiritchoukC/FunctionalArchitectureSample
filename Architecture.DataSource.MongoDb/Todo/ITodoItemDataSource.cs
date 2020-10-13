
using System.Threading;
using System.Threading.Tasks;
using Architecture.Domain.Common.Database;
using LanguageExt;
using LanguageExt.Common;

namespace Architecture.DataSource.MongoDb.Todo
{
    using System;
    using System.Collections.Generic;
    using Architecture.Domain.Todo;

    public interface ITodoItemDataSource
    {
        Either<DatabaseFailure, IEnumerable<TodoItemDto>> GetAll();
        Either<DatabaseFailure, Option<TodoItem>> GetById(Guid id);
        Either<DatabaseFailure, Unit> Add(TodoItemDto todoItem);
        Either<DatabaseFailure, Unit> Update(TodoItemDto todoItem);
    }
}