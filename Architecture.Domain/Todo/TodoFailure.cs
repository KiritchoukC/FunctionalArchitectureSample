using Architecture.Domain.Common.Cache;
using Architecture.Domain.Common.Database;
using LanguageExt;

namespace Architecture.Domain.Todo
{
    //
    // public abstract class TodoFailure : OneOfBase<
    //     TodoFailure.Cache, 
    //     TodoFailure.Network>
    // {
    //     public virtual string Message { get; }
    //     
    //     public class Cache : TodoFailure
    //     {
    //         public Cache(CacheFailure cacheFailure)
    //         {
    //             Message = cacheFailure.Message;
    //         }
    //
    //         public override string Message { get; }
    //     }
    //     
    //     public class Network : TodoFailure
    //     {
    //         public override string Message => "An error occured with the network";
    //     }
    // }
    
    [Union]
    public abstract partial class TodoFailure
    {
        public abstract TodoFailure Cache(CacheFailure cacheFailure);
        public abstract TodoFailure Database(DatabaseFailure databaseFailure);
    }
}