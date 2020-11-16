
namespace Architecture.DataSource.MongoDb.Todo
{
    using Architecture.Domain.Common.Database;

    using LanguageExt;

    public interface ITodoItemDataSource
    {
        EitherAsync<DatabaseFailure, Seq<TodoItemDto>> GetAllAsync();
        EitherAsync<DatabaseFailure, Unit> AddAsync(TodoItemDto todoItem);
        EitherAsync<DatabaseFailure, Unit> UpdateAsync(TodoItemDto todoItem);
    }
}