using System;
using LanguageExt;

namespace Architecture.Domain.Todo
{
    // public class TodoContent
    // {
    //     private TodoContent(string value)
    //     {
    //         Value = value;
    //     }
    //     
    //     public static TodoContent New(string value)
    //     {
    //         return new TodoContent(value);
    //     }
    //
    //     public string Value { get; }
    // }
    //
    // public class TodoItem
    // {
    //     private TodoItem(Guid id, bool isDone, TodoContent content)
    //     {
    //         Id = id;
    //         IsDone = isDone;
    //         Content = content;
    //     }
    //
    //     public static TodoItem New(Guid id, bool isDone, string content)
    //     {
    //         return new TodoItem(
    //             id,
    //             isDone,
    //             TodoContent.New(content));
    //     }
    //     
    //     public Guid Id { get; }
    //     public bool IsDone { get; }
    //     public TodoContent Content { get; }
    // }

    [Record]
    public partial struct TodoContent
    {
        public readonly string Value;
    }
    
    [Record]
    public partial struct TodoItem
    {
        public readonly Guid Id;
        public readonly bool IsDone;
        public readonly TodoContent Content;
    }
}