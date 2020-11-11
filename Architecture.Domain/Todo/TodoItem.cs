namespace Architecture.Domain.Todo
{
    using System;

    public record TodoContent(string Value);
    public record TodoId(Guid Value);
    public record TodoIsDone(bool Value);
    public record TodoItem(TodoId Id, TodoIsDone IsDone, TodoContent Content);
}