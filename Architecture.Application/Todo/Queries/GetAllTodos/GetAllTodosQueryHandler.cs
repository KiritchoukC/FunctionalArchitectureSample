using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Architecture.Domain.Todo;
using LanguageExt;
using MediatR;

namespace Architecture.Application.Todo.Queries.GetAllTodos
{
    public class GetAllTodosQuery : IRequest<Either<TodoFailure, ImmutableList<TodoItem>>> { }
    
    public class GetAllTodosQueryHandler : IRequestHandler<GetAllTodosQuery, Either<TodoFailure, ImmutableList<TodoItem>>>
    {
        public Task<Either<TodoFailure, ImmutableList<TodoItem>>> Handle(GetAllTodosQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}