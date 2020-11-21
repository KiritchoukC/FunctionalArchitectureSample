using System.Collections.Generic;
using System.Threading.Tasks;
using Architecture.Domain.Todo;
using LanguageExt;

namespace Architecture.Infrastructure.Todo
{
    public interface ITodoItemRepository
    {
        EitherAsync<TodoFailure, Seq<TodoItem>> GetAll();
        EitherAsync<TodoFailure, Option<TodoItem>> GetById(TodoId id);
        EitherAsync<TodoFailure, Unit> Add(TodoItem item);
    }
}