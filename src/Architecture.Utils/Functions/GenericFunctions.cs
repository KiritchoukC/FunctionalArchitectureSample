namespace Architecture.Utils.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Architecture.Domain.Todo;
    using LanguageExt;
    using static LanguageExt.Prelude;

    public static class GenericFunctions
    {
        public static TResult Use<TUse, TResult>(TUse objectToUse, Func<TUse, TResult> f) where TUse: IDisposable
        {
            using var o = objectToUse;
            return f(o);
        }

        public static Either<TFailure, TResult> Right<TFailure, TResult>(TResult value) => Prelude.Right(value);
    }
}
