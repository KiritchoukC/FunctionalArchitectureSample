
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
        public static Func<string, Validation<string, string>> MaxStrLength(int max) =>
            str =>
                str.Length <= max
                    ? Success<string, string>(str)
                    : Fail<string, string>($"field can not exceed {max} characters");

        public static Validation<string, string> NotEmpty(string str) =>
            string.IsNullOrWhiteSpace(str)
                ? Fail<string, string>("field cannot be empty")
                : Success<string, string>(str);

        public static Validation<string, string> NotNull(string? value) =>
            value is not null
                ? Success<string, string>(value)
                : Fail<string, string>("field cannot be null");
    }
}
