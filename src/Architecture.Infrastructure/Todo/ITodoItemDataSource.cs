
namespace Architecture.Infrastructure.Todo
{
    using Architecture.Domain.Common.Database;

    using LanguageExt;

    public interface ITodoItemDataSource
    {
        EitherAsync<DatabaseFailure, Seq<TodoItemDto>> GetAll();
        EitherAsync<DatabaseFailure, Unit> Add(TodoItemDto todoItem);
        EitherAsync<DatabaseFailure, Unit> Update(TodoItemDto todoItem);
    }
}
