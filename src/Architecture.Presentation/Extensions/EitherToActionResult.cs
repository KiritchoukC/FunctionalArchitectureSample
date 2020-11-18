namespace Architecture.Presentation.Extensions
{
    using System.Threading.Tasks;
    using Architecture.Domain.Todo;
    using LanguageExt;
    using Microsoft.AspNetCore.Mvc;

    using static Architecture.Presentation.Common.FailuresHandlers;

    public static class EitherToActionResult
    {
        public static Task<IActionResult> ToActionResult<TResult>(this Task<Either<TodoFailure, TResult>> either) =>
            either
                .ToAsync()
                .MapLeft(HandleFailure)
                .Match<IActionResult>(
                    result => result is Unit ? new OkResult() : new OkObjectResult(result),
                    failureMessage => new BadRequestObjectResult(failureMessage));
    }
}
