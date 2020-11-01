using System.Collections.Generic;
using System.Threading.Tasks;
using Architecture.Domain.Todo;
using LanguageExt;

namespace Architecture.Infrastructure.Todo
{
    public interface ITodoItemRepository
    {
        Task<Either<TodoFailure, Seq<TodoItem>>> GetAllAsync();
        Task<Either<TodoFailure, Option<TodoItem>>> GetByIdAsync(TodoId id);
        Task<Either<TodoFailure, Unit>> AddAsync(TodoItem item);
    }
}