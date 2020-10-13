using Architecture.DataSource.MongoDb.Todo;
using Architecture.Domain.Todo;

namespace Architecture.Infrastructure.Todo
{
    public static class TodoItemTranslator
    {
        public static TodoItem FromDto(TodoItemDto dto) 
            => TodoItem.New(dto.Id, dto.IsDone, TodoContent.New(dto.Content));
    }
}