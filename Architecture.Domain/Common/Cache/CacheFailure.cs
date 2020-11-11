
namespace Architecture.Domain.Common.Cache
{
    using OneOf;

    public abstract class CacheFailure : OneOfBase<
        CacheFailure.Fetch,
        CacheFailure.Insert,
        CacheFailure.Decoding,
        CacheFailure.Encoding,
        CacheFailure.Serialization,
        CacheFailure.Deserialization>
    {
        public class Fetch : CacheFailure { }
        public class Insert : CacheFailure { }
        public class Decoding : CacheFailure { }
        public class Encoding : CacheFailure { }
        public class Serialization : CacheFailure { }
        public class Deserialization : CacheFailure { }
    }
}