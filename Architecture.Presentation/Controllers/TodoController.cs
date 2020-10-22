using System;
using System.Threading.Tasks;
using Architecture.Application.Todo.Queries.GetAllTodos;
using Architecture.Domain.Todo;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using static Architecture.Presentation.Common.FailuresHandlers;

namespace Architecture.Presentation.Controllers
{
    [ApiController]
    [Route("api/todo")]
    public class TodoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TodoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllTodosQuery());

            return result.Match(
                items => Ok(items),
                HandleFailure);
        }

        private IActionResult HandleFailure(TodoFailure failure)
            => failure switch
            {
                Cache cache => Problem(HandleCacheFailure(cache.CacheFailure)),
                Database database => Problem(HandleDatabaseFailure(database.DatabaseFailure)),
                _ => throw new ArgumentOutOfRangeException(nameof(failure))
            };
    }
}