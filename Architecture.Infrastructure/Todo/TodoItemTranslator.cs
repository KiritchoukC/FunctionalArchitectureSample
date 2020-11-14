
namespace Architecture.Infrastructure.Todo
{
    using Architecture.DataSource.MongoDb.Todo;
    using Architecture.Domain.Todo;

    using LanguageExt;
    using LanguageExt.Common;

    public static class TodoItemTranslator
    {
        public static Validation<Error, TodoItem> FromDto(TodoItemDto dto)
            => TodoItem.New(dto.Id, dto.IsDone, dto.Content);

        public static TodoItemDto ToDto(TodoItem item)
            => TodoItemDto.New(item.Id.Value, item.IsDone.Value, item.Content.Value);
    }
}