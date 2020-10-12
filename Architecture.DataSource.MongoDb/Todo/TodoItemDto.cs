using System;

namespace Architecture.DataSource.MongoDb.Todo
{
    public class TodoItemDto
    {
        public Guid Id { get; set; }
        public bool IsDone { get; set; }
        public string Content { get; set; }
    }
}