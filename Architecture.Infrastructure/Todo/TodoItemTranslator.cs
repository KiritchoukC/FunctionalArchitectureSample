using Architecture.DataSource.MongoDb.Todo;
using Architecture.Domain.Todo;
using static Architecture.Domain.Todo.TodoConstructors;

namespace Architecture.Infrastructure.Todo
{
    public static class TodoItemTranslator
    {
        public static TodoItem FromDto(TodoItemDto dto)
            => TodoItem(TodoId(dto.Id), TodoIsDone(dto.IsDone), TodoContent(dto.Content));

        public static TodoItemDto ToDto(TodoItem item)
            => TodoItemDto.New(item.Id.Value, item.IsDone.Value, item.Content.Value);
    }
}