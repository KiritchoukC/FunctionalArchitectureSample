using OneOf;

namespace Architecture.Domain.Common.Cache
{
    public static class CacheFailures {
        public static CacheFailure Retrieve() => new CacheFailure.Retrieve();
        public static CacheFailure Insert() => new CacheFailure.Insert();
        public static CacheFailure Decoding() => new CacheFailure.Decoding();
        public static CacheFailure Encoding() => new CacheFailure.Encoding();
        public static CacheFailure Serialization() => new CacheFailure.Serialization();
        public static CacheFailure Deserialization() => new CacheFailure.Deserialization();
    }
    
    public abstract class CacheFailure : OneOfBase<
        CacheFailure.Retrieve, 
        CacheFailure.Insert, 
        CacheFailure.Decoding, 
        CacheFailure.Encoding, 
        CacheFailure.Serialization, 
        CacheFailure.Deserialization>
    {
        public virtual string Message { get; }
        
        public class Retrieve : CacheFailure
        {
            public override string Message => "An error occured while getting item from cache";
        }
        
        public class Insert : CacheFailure
        {
            public override string Message => "An error occured while inserting item in cache";
        }

        public class Decoding : CacheFailure
        {
            public override string Message => "An error occured with the cache decoding";
        }

        public class Encoding : CacheFailure
        {
            public override string Message => "An error occured with the cache encoding";
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