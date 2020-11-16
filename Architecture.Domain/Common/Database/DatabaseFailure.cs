namespace Architecture.Domain.Common.Database
{
    using LanguageExt.Common;

    public abstract class DatabaseFailure
    {
        public Error Error { get; }
        public DatabaseFailure(Error error) => Error = error;

        public class Retrieve : DatabaseFailure { public Retrieve(Error error) : base(error) { } }
        public class Insert : DatabaseFailure { public Insert(Error error) : base(error) { } }
    }
}