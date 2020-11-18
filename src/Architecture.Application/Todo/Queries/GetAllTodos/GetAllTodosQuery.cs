namespace Architecture.Application.Todo.Queries.GetAllTodos
{
    using System.Collections.Immutable;
    using System.Threading;
    using System.Threading.Tasks;
    using Architecture.Domain.Todo;
    using Architecture.Infrastructure.Todo;
    using Architecture.Utils.Extensions;

    using LanguageExt;
    using MediatR;

    using Microsoft.Extensions.Logging;

    public class GetAllTodosQuery : IRequest<Either<TodoFailure, Seq<TodoItemModel>>> { }

    public class GetAllTodosQueryHandler : IRequestHandler<GetAllTodosQuery, Either<TodoFailure, Seq<TodoItemModel>>>
    {
        private readonly ITodoItemRepository _todoItemRepository;
        private readonly ILogger _logger;

        public GetAllTodosQueryHandler(
            ITodoItemRepository todoItemRepository,
            ILogger logger)
        {
            _todoItemRepository = todoItemRepository;
            _logger = logger;
        }

        public Task<Either<TodoFailure, Seq<TodoItemModel>>> Handle(GetAllTodosQuery request, CancellationToken token) =>
            Fetch()
                .Map(Project)
                .LogWarningLeft(_logger, LogFailure)
                .ToEither();

        private EitherAsync<TodoFailure, Seq<TodoItem>> Fetch() => _todoItemRepository.GetAllAsync();

        private static Seq<TodoItemModel> Project(Seq<TodoItem> items) =>
            items.Select(x => new TodoItemModel(x.Id.Value, x.Content.Value, x.IsDone.Value));

        private static string LogFailure(TodoFailure failure) =>
            failure switch {
                TodoFailure.Cache f => f.Failure.Error.ToString(),
                TodoFailure.Database f => f.Failure.Error.ToString(),
                TodoFailure.Translation f => f.ErrorsJoined,
                TodoFailure.Validation f => f.ErrorsJoined,
                _ => "Unhandled failure"
            };
    }
}