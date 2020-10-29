using System.Threading.Tasks;
using Architecture.Domain.Todo;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;

using static Architecture.Presentation.Common.FailuresHandlers;

namespace Architecture.Presentation.Extensions
{
    public static class EitherToActionResult
    {
        public static Task<IActionResult> ToActionResult<L, R>(this Task<Either<L, R>> either) =>
            either.Map(Match);

        public static Task<IActionResult> ToActionResult(this Task<Either<TodoFailure, Task>> either) =>
            either.Bind(Match);

        private static IActionResult Match<L, R>(this Either<L, R> either) =>
            either.Match<IActionResult>(
                Left: l => new BadRequestObjectResult(l),
                Right: r => new OkObjectResult(r));

        private static async Task<IActionResult> Match(Either<TodoFailure, Task> either) =>
            await either.MatchAsync<IActionResult>(
                RightAsync: async t => { await t; return new OkResult(); },
                Left: f => new BadRequestObjectResult(HandleFailure(f)));
    }
}
