using System.Collections.Generic;
using System.Threading.Tasks;
using Architecture.Domain.Todo;
using LanguageExt;

namespace Architecture.Infrastructure.Todo
{
    public interface ITodoItemRepository
    {
        EitherAsync<TodoFailure, Seq<TodoItem>> GetAllAsync();
        EitherAsync<TodoFailure, Option<TodoItem>> GetByIdAsync(TodoId id);
        EitherAsync<TodoFailure, Unit> AddAsync(TodoItem item);
    }
}