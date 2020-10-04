using OneOf;

namespace Architecture.Domain.Todo
{
    public static class TodoFailures {
        public static TodoFailure Cache() => new TodoFailure.Cache();
        public static TodoFailure CacheDecoding() => new TodoFailure.CacheDecoding();
        public static TodoFailure CacheSerialization() => new TodoFailure.CacheSerialization();
        public static TodoFailure CacheDeserialization() => new TodoFailure.CacheDeserialization();
        public static TodoFailure Network() => new TodoFailure.Network();
    }
    
    public abstract class TodoFailure : OneOfBase<
        TodoFailure.Cache, 
        TodoFailure.CacheDecoding, 
        TodoFailure.CacheSerialization, 
        TodoFailure.CacheDeserialization, 
        TodoFailure.Network>
    {
        
        public class Cache : TodoFailure
        {
            public string Message => "An error occured with the cache system";
        }

        public class CacheDecoding : TodoFailure
        {
            public string Message => "An error occured with the cache decoding";
        }

        public class CacheSerialization : TodoFailure
        {
            public string Message => "An error occured with the cache serialization";
        }

        public class CacheDeserialization : TodoFailure
        {
            public string Message => "An error occured with the cache deserialization";
        }

        public class Network : TodoFailure
        {
            public string Message => "An error occured with the network";
        }
    }
}