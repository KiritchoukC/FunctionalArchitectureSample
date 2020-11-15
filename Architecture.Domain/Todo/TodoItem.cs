namespace Architecture.Domain.Todo
{
    using System;

    using LanguageExt;
    using LanguageExt.Common;

    using static LanguageExt.Prelude;
    using static Validators.Validators;

    public record TodoContent
    {
        public string Value { get; }

        private TodoContent(string value) => Value = value;

        private static Validation<string, string> Validation(string? value) =>
            NotNull(value)
                .Bind(NotEmpty)
                .Bind(MaxStrLength(300));

        private static Validation<Error, string> Validate(string? value) =>
            Validation(value)
                .MapFail(error => Error.New("Content " + error));

        public static Validation<Error, TodoContent> New(string? value) =>
            Validate(value)
                .Map(str => new TodoContent(str));
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

        private static Validation<string, bool> Validation(bool? value) =>
            NotNull(value);

        private static Validation<Error, bool> Validate(bool? value) =>
            Validation(value)
                .MapFail(error => Error.New("IsDone " + error));

        public static Validation<Error, TodoIsDone> New(bool? value) =>
            Validate(value)
                .Map(x => new TodoIsDone(x));
    }

    public record TodoItem
    {
        public TodoId Id { get; }
        public TodoIsDone IsDone { get; }
        public TodoContent Content { get; }

        private TodoItem(TodoId id, TodoIsDone isDone, TodoContent content) =>
            (Id, IsDone, Content) = (id, isDone, content);

        public static Validation<Error, TodoItem> New(Guid id, bool? isDone, string? content) =>
            (TodoId.New(id), TodoIsDone.New(isDone), TodoContent.New(content))
                .Apply((id, isDone, content) => new TodoItem(id, isDone, content));
    }
}