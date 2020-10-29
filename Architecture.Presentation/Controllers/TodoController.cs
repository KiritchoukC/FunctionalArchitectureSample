using System;
using System.Threading.Tasks;
using Architecture.Application.Todo.Queries.GetAllTodos;
using Architecture.Domain.Todo;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Architecture.Presentation.Extensions;

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
        public Task<IActionResult> GetAll() => _mediator.Send(new GetAllTodosQuery()).ToActionResult();
    }
}