using System;

namespace Architecture.DataSource.MongoDb.Todo
{
    public class TodoItemDto
    {
        private TodoItemDto(Guid id, bool isDone, string content)
        {
            Id = id;
            IsDone = isDone;
            Content = content;
        }
        public static TodoItemDto New(Guid id, bool isDone, string content)
        {
            return new TodoItemDto(id, isDone, content);
        }

        public Guid Id { get; }
        public bool IsDone { get; }
        public string Content { get; }
    }
}