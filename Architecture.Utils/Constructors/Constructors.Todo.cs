
namespace Architecture.Utils.Constructors
{
    using System;

    using Architecture.Domain.Todo;

    public static partial class Constructors
    {
        public static TodoContent TodoContent(string content) => Architecture.Domain.Todo.TodoContent.New(content);
        public static TodoId TodoId(Guid id) => Architecture.Domain.Todo.TodoId.New(id);
        public static TodoIsDone TodoIsDone(bool isDone) => Architecture.Domain.Todo.TodoIsDone.New(isDone);

        public static TodoItem TodoItem(TodoId id, TodoIsDone isDone, TodoContent content) => Architecture.Domain.Todo.TodoItem.New(id, isDone, content);
    }
}
