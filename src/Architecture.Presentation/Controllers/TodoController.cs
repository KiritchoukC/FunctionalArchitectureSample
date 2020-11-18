namespace Architecture.Presentation.Controllers
{
    using System.Threading.Tasks;

    using Architecture.Application.Todo.Commands.AddTodo;
    using Architecture.Application.Todo.Queries.GetAllTodos;
    using Architecture.Presentation.Common;
    using Architecture.Presentation.Extensions;

    using LanguageExt;

    using MediatR;

    using Microsoft.AspNetCore.Mvc;

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
        public Task<IActionResult> GetAll() => _mediator.Send(new GetAllTodosQuery()).ToActionResult();

        [HttpPost("add")]
        public Task<IActionResult> Add(AddTodoCommand command) => _mediator.Send(command).ToActionResult();
    }
}