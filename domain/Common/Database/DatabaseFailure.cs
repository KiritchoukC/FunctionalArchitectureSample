namespace Architecture.Domain.Common.Database
{
    using System;
    using LanguageExt;
    using LanguageExt.Common;

    [Union]
    public abstract partial class DatabaseFailure
    {
        public abstract DatabaseFailure Retrieve(Error error);
    }
}