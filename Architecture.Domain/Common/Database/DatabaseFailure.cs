using System;
using LanguageExt;

namespace Architecture.Domain.Common.Database
{
    [Union]
    public abstract partial class DatabaseFailure
    {
        public abstract DatabaseFailure Retrieve(Exception ex);
    }
}