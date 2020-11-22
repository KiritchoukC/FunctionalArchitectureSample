using MediatR;

namespace Architecture.Application.Todo.Commands.AddTodo
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Architecture.Domain.Todo;
    using Architecture.Infrastructure.Todo;

    using LanguageExt;

    using static Utils.Constructors;

    public record AddTodoCommand(string? Content, bool? IsDone) : IRequest<Either<TodoFailure, Unit>>;

    public class AddTodoCommandHandler : IRequestHandler<AddTodoCommand, Either<TodoFailure, Unit>>
    {
        private readonly ITodoItemRepository _todoItemRepository;

        public AddTodoCommandHandler(ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }

        public async Task<Either<TodoFailure, Unit>> Handle(AddTodoCommand request, CancellationToken cancellationToken) =>
            await
            (from item in Validate(request)
             from _ in _todoItemRepository.Add(item)
             select _).ToEither();

        private static EitherAsync<TodoFailure, TodoItem> Validate(AddTodoCommand request) =>
            TodoItem.New(Guid.NewGuid(), request.IsDone, request.Content)
                .ToEither().ToAsync()
                .MapLeft(TodoFailureCon.Validation);
    }
}
