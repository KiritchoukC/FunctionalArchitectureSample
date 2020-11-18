
namespace Architecture.Domain.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using LanguageExt;
    using LanguageExt.Common;

    using static LanguageExt.Prelude;

    public static partial class Validators
    {

        public static Validation<string, bool> NotNull(bool? value) =>
            value.HasValue
                ? Success<string, bool>(value.Value)
                : Fail<string, bool>("field cannot be null");
    }
}
