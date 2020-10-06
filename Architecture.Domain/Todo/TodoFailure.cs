using Architecture.Domain.Common.Cache;
using OneOf;

namespace Architecture.Domain.Todo
{
    public static class TodoFailures {
        public static TodoFailure Cache(CacheFailure cacheFailure) => new TodoFailure.Cache(cacheFailure);
        public static TodoFailure Network() => new TodoFailure.Network();
    }
    
    public abstract class TodoFailure : OneOfBase<
        TodoFailure.Cache, 
        TodoFailure.Network>
    {
        public virtual string Message { get; }
        
        public class Cache : TodoFailure
        {
            public Cache(CacheFailure cacheFailure)
            {
                Message = cacheFailure.Message;
            }

            public override string Message { get; }
        }
        
        public class Network : TodoFailure
        {
            public override string Message => "An error occured with the network";
        }
    }
}