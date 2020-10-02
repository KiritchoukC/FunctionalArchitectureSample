using OneOf;

namespace Architecture.Domain.Todo
{
    public abstract class TodoFailure : OneOfBase<TodoFailure.Cache, TodoFailure.Network>
    {
        public class Cache : TodoFailure
        {
            public string Message => "An error occured with the cache system";
        }

        public class Network : TodoFailure
        {
            public string Message => "An error occured with the network";
        }
    }
}