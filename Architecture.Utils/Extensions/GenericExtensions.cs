namespace Architecture.Utils.Extensions
{
    using System;
    using LanguageExt;
    using Microsoft.Extensions.Logging;
    using static LanguageExt.Prelude;

    public static class GenericExtensions
    {
        public static T LogInfo<T>(this T @this, ILogger logger, Func<T, string> messageFunc)
        {
            logger.LogInformation(messageFunc(@this));
            return @this;
        }

        public static Either<TFailure, Option<T>> BindO<TFailure, T>(
            this Either<TFailure, Option<T>> @this,
            Func<T, Either<TFailure, T>> f)
            => @this.Bind(opt => opt.Match(t => f(t).Map(Optional), () => Right(Option<T>.None)));

        public static Either<TFailure, Option<TResult>> MapO<TFailure, TSource, TResult>(
            this Either<TFailure, Option<TSource>> @this,
            Func<TSource, Either<TFailure, TResult>> f)
            => @this.Bind(opt => opt.Match(t => f(t).Map(Optional), () => Right(Option<TResult>.None)));
    }
}