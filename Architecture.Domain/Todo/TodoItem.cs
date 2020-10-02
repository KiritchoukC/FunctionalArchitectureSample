using System;

namespace Architecture.Domain.Todo
{
    public class TodoContent
    {
        private TodoContent(string value)
        {
            Value = value;
        }
        
        public static TodoContent CreateInstance(string value)
        {
            return new TodoContent(value);
        }

        public string Value { get; }
    }
    
    public class TodoItem
    {
        private TodoItem(Guid id, bool isDone, TodoContent content)
        {
            Id = id;
            IsDone = isDone;
            Content = content;
        }

        public static TodoItem CreateInstance(Guid id, bool isDone, string content)
        {
            return new TodoItem(
                id,
                isDone,
                TodoContent.CreateInstance(content));
        }
        
        public Guid Id { get; }
        public bool IsDone { get; }
        public TodoContent Content { get; }
    }
}