namespace Architecture.Domain.Common.Database
{
    using LanguageExt.Common;

    using OneOf;

    public abstract class DatabaseFailure : OneOfBase<DatabaseFailure.Retrieve>
    {
        public Error Error { get; }
        public DatabaseFailure(Error error) => Error = error;

        public class Retrieve : DatabaseFailure
        {
            public Retrieve(Error error) : base(error) { }
        }
    }
}