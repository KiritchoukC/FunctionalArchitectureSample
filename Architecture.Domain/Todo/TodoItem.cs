using System;
using LanguageExt;

namespace Architecture.Domain.Todo
{
    public static class TodoConstructors
    {
        public static TodoContent TodoContent(string content) => Todo.TodoContent.New(content);
        public static TodoId TodoId(Guid id) => Todo.TodoId.New(id);
        public static TodoIsDone TodoIsDone(bool isDone) => Todo.TodoIsDone.New(isDone);
        public static TodoItem TodoItem(TodoId id,TodoIsDone isDone,TodoContent content) => Todo.TodoItem.New(id, isDone, content);
    }

    [Record]
    public partial struct TodoContent
    {
        public readonly string Value;
    }

    [Record]
    public partial struct TodoId
    {
        public readonly Guid Value;
    }

    [Record]
    public partial struct TodoIsDone
    {
        public readonly bool Value;
    }

    [Record]
    public partial struct TodoItem
    {
        public readonly TodoId Id;
        public readonly TodoIsDone IsDone;
        public readonly TodoContent Content;
    }
}