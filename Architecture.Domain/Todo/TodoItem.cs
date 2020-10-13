using System;
using LanguageExt;

namespace Architecture.Domain.Todo
{
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