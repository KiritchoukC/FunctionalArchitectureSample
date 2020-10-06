using OneOf;

namespace Architecture.Domain.Common.Cache
{
    public static class CacheFailures {
        public static CacheFailure Retrieve() => new CacheFailure.Retrieve();
        public static CacheFailure Decoding() => new CacheFailure.Decoding();
        public static CacheFailure Serialization() => new CacheFailure.Serialization();
        public static CacheFailure Deserialization() => new CacheFailure.Deserialization();
    }
    
    public abstract class CacheFailure : OneOfBase<
        CacheFailure.Retrieve, 
        CacheFailure.Decoding, 
        CacheFailure.Serialization, 
        CacheFailure.Deserialization>
    {
        public virtual string Message { get; }
        
        public class Retrieve : CacheFailure
        {
            public override string Message => "An error occured with the cache system";
        }

        public class Decoding : CacheFailure
        {
            public override string Message => "An error occured with the cache decoding";
        }

        public class Serialization : CacheFailure
        {
            public override string Message => "An error occured with the cache serialization";
        }

        public class Deserialization : CacheFailure
        {
            public override string Message => "An error occured with the cache deserialization";
        }
    }
}