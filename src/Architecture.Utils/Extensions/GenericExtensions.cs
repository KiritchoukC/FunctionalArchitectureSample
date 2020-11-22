namespace Architecture.Utils
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

        public static Either<TLeft, TRight> LogWarningLeft<TLeft, TRight>(this Either<TLeft, TRight> @this, ILogger logger, Func<TLeft, string> messageFunc)
        {
            if (@this.IsLeft)
            {
                logger.LogWarning(@this.Match(_ => "", messageFunc));
            }
            return @this;
        }

        public static EitherAsync<TLeft, TRight> LogWarningLeft<TLeft, TRight>(this EitherAsync<TLeft, TRight> @this, ILogger logger, Func<TLeft, string> messageFunc)
        {
            @this.Match(
                _ => unit,
                leftCase =>
                {
                    logger.LogWarning(messageFunc(leftCase));
                    return unit;
                });
            return @this;
        }

        public static Either<TFailure, Option<TResult>> BindO<TFailure, TResult>(
            this Either<TFailure, Option<TResult>> @this,
            Func<TResult, Either<TFailure, TResult>> f)
            => @this.Bind(opt => opt.Match(t => f(t).Map(Optional), () => Right<TFailure, Option<TResult>>(Option<TResult>.None)));

        public static Either<TFailure, Option<TResult>> MapO<TFailure, TSource, TResult>(
            this Either<TFailure, Option<TSource>> @this,
            Func<TSource, Either<TFailure, TResult>> f)
            => @this.Bind(opt => opt.Match(t => f(t).Map(Optional), () => Right<TFailure, Option<TResult>>(Option<TResult>.None)));

        public static EitherAsync<TFailure, Option<TResult>> BindO<TFailure, TResult>(
            this EitherAsync<TFailure, Option<TResult>> @this,
            Func<TResult, EitherAsync<TFailure, TResult>> f)
            => @this.Bind(opt => opt.Match(t => f(t).Map(Optional), () => Right<TFailure, Option<TResult>>(Option<TResult>.None).ToAsync()));

        public static EitherAsync<TFailure, Option<TResult>> MapO<TFailure, TSource, TResult>(
            this EitherAsync<TFailure, Option<TSource>> @this,
            Func<TSource, EitherAsync<TFailure, TResult>> f)
            => @this.Bind(opt => opt.Match(t => f(t).Map(Optional), () => Right<TFailure, Option<TResult>>(Option<TResult>.None).ToAsync()));

    }
}