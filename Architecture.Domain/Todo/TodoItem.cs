namespace Architecture.Domain.Todo
{
    using System;

    using LanguageExt;
    using LanguageExt.Common;

    using static LanguageExt.Prelude;

    public record TodoContent
    {
        public string Value { get; }

        private TodoContent(string value) => Value = value;

        public static Validation<Error, TodoContent> New(string value) =>
            string.IsNullOrWhiteSpace(value) ? Error.New("Todo content should not be empty.")
            : value.Length > 300 ? Error.New("Todo content should not be empty.")
            : Success<Error, TodoContent>(new(value));
    }

    public record TodoId
    {
        public Guid Value { get; }

        private TodoId(Guid value) => Value = value;

        public static Validation<Error, TodoId> New(Guid value) =>
            Success<Error, TodoId>(new(value));
    }

    public record TodoIsDone
    {
        public bool Value { get; }

        private TodoIsDone(bool value) => Value = value;

        public static Validation<Error, TodoIsDone> New(bool value) =>
            Success<Error, TodoIsDone>(new(value));
    }

    public record TodoItem
    {
        public TodoId Id { get; }
        public TodoIsDone IsDone { get; }
        public TodoContent Content { get; }

        private TodoItem(TodoId id, TodoIsDone isDone, TodoContent content) =>
            (Id, IsDone, Content) = (id, isDone, content);

        public static Validation<Error, TodoItem> New(Guid id, bool isDone, string content) =>
            from todoId in TodoId.New(id)
            from todoIsDone in TodoIsDone.New(isDone)
            from todoContent in TodoContent.New(content)
            select new TodoItem(todoId, todoIsDone, todoContent);

    }
}