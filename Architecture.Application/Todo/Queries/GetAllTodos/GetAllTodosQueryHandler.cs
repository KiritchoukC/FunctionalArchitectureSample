using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Architecture.Domain.Todo;
using Architecture.Infrastructure.Todo;
using LanguageExt;
using MediatR;

namespace Architecture.Application.Todo.Queries.GetAllTodos
{
    public class GetAllTodosQuery : IRequest<Either<TodoFailure, Seq<TodoItem>>> { }
    
    public class GetAllTodosQueryHandler : IRequestHandler<GetAllTodosQuery, Either<TodoFailure, Seq<TodoItem>>>
    {
        private readonly ITodoItemRepository _todoItemRepository;

        public GetAllTodosQueryHandler(ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }

        public Task<Either<TodoFailure, Seq<TodoItem>>> Handle(GetAllTodosQuery request, CancellationToken token)
        {
            return _todoItemRepository.GetAll().AsTask();
        }
    }
}