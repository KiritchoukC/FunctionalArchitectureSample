
namespace Architecture.Utils.Constructors
{
    using System;

    using Architecture.Domain.Todo;

    public static partial class Constructors
    {
        public static TodoContent TodoContent(string content) => new(content);
        public static TodoId TodoId(Guid id) => new(id);
        public static TodoIsDone TodoIsDone(bool isDone) => new(isDone);

        public static TodoItem TodoItem(TodoId id, TodoIsDone isDone, TodoContent content) => new(id, isDone, content);
    }
}
