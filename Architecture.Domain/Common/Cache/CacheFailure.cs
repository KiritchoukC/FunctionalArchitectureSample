
namespace Architecture.Domain.Common.Cache
{
    using LanguageExt.Common;

    public abstract class CacheFailure
    {
        public Error Error { get; }
        public CacheFailure(Error error) => Error = error;

        public class Fetch : CacheFailure { public Fetch(Error error) : base(error) { } }
        public class Insert : CacheFailure { public Insert(Error error) : base(error) { } }
        public class Serialization : CacheFailure { public Serialization(Error error) : base(error) { } }
        public class Deserialization : CacheFailure { public Deserialization(Error error) : base(error) { } }
    }
}