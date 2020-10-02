using System.Threading.Tasks;
using Architecture.Application.Todo.Queries.GetAllTodos;
using Architecture.Domain.Todo;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Architecture.Presentation.Api.Controllers
{
    [ApiController]
    [Route("api/todo")]
    public class TodoItemController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TodoItemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllTodosQuery());

            return result.Match<IActionResult>(
                Ok,
                (failure) =>
                    failure.Match(
                        cacheFailure => BadRequest(cacheFailure.Message),
                        networkFailure => BadRequest(networkFailure.Message)));
        }
    }
}