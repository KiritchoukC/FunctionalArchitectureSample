
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
        EitherAsync<DatabaseFailure, Seq<TodoItemDto>> GetAllAsync();
        EitherAsync<DatabaseFailure, Option<TodoItem>> GetByIdAsync(Guid id);
        EitherAsync<DatabaseFailure, Unit> AddAsync(TodoItemDto todoItem);
        EitherAsync<DatabaseFailure, Unit> UpdateAsync(TodoItemDto todoItem);
    }
}