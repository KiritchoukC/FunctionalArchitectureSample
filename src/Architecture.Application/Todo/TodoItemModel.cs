using System;

namespace Architecture.Application.Todo
{
    public class TodoItemModel
    {
        public TodoItemModel(Guid id, string content, bool isDone)
        {
            Id = id;
            Content = content;
            IsDone = isDone;
        }

        public Guid Id { get; }
        public string Content { get; }
        public bool IsDone { get; }
    }
}
