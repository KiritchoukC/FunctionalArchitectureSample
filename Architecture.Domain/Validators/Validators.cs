using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Architecture.Domain.Validators
{
    public static partial class Validators
    {
        public static Validation<Error, T> NotNull<T>(T? value) =>
            notnull(value)
                ? Success<Error, T>(value)
                : Fail<Error, T>(Error.New("Value cannot be null"));
    }
}
