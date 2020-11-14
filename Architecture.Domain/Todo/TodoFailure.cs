
namespace Architecture.Domain.Todo
{
    using System;

    using Architecture.Domain.Common.Cache;
    using Architecture.Domain.Common.Database;

    using LanguageExt;
    using LanguageExt.Common;

    using OneOf;

    public abstract class TodoFailure : OneOfBase<
        TodoFailure.Cache,
        TodoFailure.Database,
        TodoFailure.Validation,
        TodoFailure.Translation>
    {
        public class Cache : TodoFailure
        {
            public readonly CacheFailure Failure;
            public Cache(CacheFailure failure) => Failure = failure;
        }
        public class Database : TodoFailure
        {
            public readonly DatabaseFailure Failure;
            public Database(DatabaseFailure failure) => Failure = failure;
        }
        public class Validation : TodoFailure
        {
            public readonly Seq<Error> Errors;
            public string ErrorsJoined() => string.Join(Environment.NewLine, Errors);
            public Validation(Seq<Error> errors) => Errors = errors;
        }
        public class Translation : TodoFailure
        {
            public readonly Seq<Error> Errors;
            public string ErrorsJoined() => string.Join(Environment.NewLine, Errors);
            public Translation(Seq<Error> errors) => Errors = errors;
        }
    }
}