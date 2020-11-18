namespace Architecture.Utils.Extensions
{
    using LanguageExt;

    using static LanguageExt.Prelude;

    public static class OptionExtensions
    {
        public static Seq<T> AddOrInit<T>(this Option<Seq<T>> source, T item) where T : struct
        {
            return source.Match(
                xs => xs.Add(item),
                () => Seq<T>(item));
        }
    }
}
