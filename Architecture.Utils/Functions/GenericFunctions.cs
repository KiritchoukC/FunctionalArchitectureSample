using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Architecture.Utils.Functions
{
    public static class GenericFunctions
    {
        public static TResult Use<TUse, TResult>(TUse objectToUse, Func<TUse, TResult> f) where TUse: IDisposable
        {
            using var o = objectToUse;
            return f(o);
        }

    }
}
