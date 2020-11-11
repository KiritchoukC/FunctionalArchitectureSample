using LanguageExt;

namespace Architecture.Domain.Common.Cache
{
    [Union]
    public abstract partial class CacheFailure
    {
        public abstract CacheFailure Fetch();
        public abstract CacheFailure Insert();
        public abstract CacheFailure Decoding();
        public abstract CacheFailure Encoding();
        public abstract CacheFailure Serialization();
        public abstract CacheFailure Deserialization();
    }
}