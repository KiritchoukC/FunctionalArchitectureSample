using System.Collections.Generic;
using Architecture.Domain.Todo;
using LanguageExt;

namespace Architecture.Infrastructure.Todo
{
    public interface ITodoItemRepository
    {
        Either<TodoFailure, Seq<TodoItem>> GetAll();
        Either<TodoFailure, Option<TodoItem>> GetById(TodoId id);
        Either<TodoFailure, Unit> Add(TodoItem item);
    }
}