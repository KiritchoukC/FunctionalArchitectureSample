namespace Architecture.Domain.Common.Database
{
    using LanguageExt.Common;

    using OneOf;

    public abstract class DatabaseFailure : OneOfBase<DatabaseFailure.Retrieve>
    {
        public class Retrieve : DatabaseFailure
        {
            public readonly Error Error;
            public Retrieve(Error error) => Error = error;
        }
    }
}