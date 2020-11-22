namespace Architecture.Infrastructure.Todo
{
    using System;

    using Architecture.Domain.Todo;

    using LanguageExt;
    using LanguageExt.Common;

    public class TodoItemDto
    {
        public TodoItemDto(Guid id, bool isDone, string content)
        {
            Id = id;
            IsDone = isDone;
            Content = content;
        }

        public Guid Id { get; }
        public bool IsDone { get; }
        public string Content { get; }

        public static Validation<Error, TodoItem> ToDomain(TodoItemDto dto)
            => TodoItem.New(dto.Id, dto.IsDone, dto.Content);

        public static TodoItemDto FromDomain(TodoItem item)
            => new(item.Id.Value, item.IsDone.Value, item.Content.Value);
    }
}
