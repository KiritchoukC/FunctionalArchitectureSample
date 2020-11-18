using System;

namespace Architecture.DataSource.MongoDb.Todo
{
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
    }
}