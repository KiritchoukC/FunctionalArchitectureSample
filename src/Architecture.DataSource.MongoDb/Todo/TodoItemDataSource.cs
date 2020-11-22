
namespace Architecture.DataSource.MongoDb.Todo
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Architecture.Domain.Common.Database;
    using Architecture.Infrastructure.Todo;

    using LanguageExt;
    using LanguageExt.Common;

    using MongoDB.Driver;

    using static Architecture.Utils.Constructors;
    using static LanguageExt.Prelude;

    public class TodoItemDataSource : ITodoItemDataSource
    {
        private readonly IMongoCollection<TodoItemDto> _todoCollection;

        public TodoItemDataSource(IMongoClient mongoClient)
        {
            _todoCollection = mongoClient
                .GetDatabase("todo")
                .GetCollection<TodoItemDto>("items");
        }

        public EitherAsync<DatabaseFailure, Seq<TodoItemDto>> GetAll() =>
            GetAsync(() => _todoCollection.FindAsync(x => true))
                .MapLeft(DatabaseFailureCon.Retrieve);

        public EitherAsync<DatabaseFailure, Unit> Add(TodoItemDto todoItem) =>
            TryAsync(() => _todoCollection.InsertOneAsync(todoItem).ToUnit())
                .ToEither()
                .MapLeft(DatabaseFailureCon.Insert);

        public EitherAsync<DatabaseFailure, Unit> Update(TodoItemDto todoItem) =>
            TryAsync(() => _todoCollection.UpdateOneAsync(x => x.Id == todoItem.Id, Updater(todoItem)).ToUnit())
                .ToEither()
                .MapLeft(DatabaseFailureCon.Update);

        private static EitherAsync<Error, Seq<T>> GetAsync<T>(Func<Task<IAsyncCursor<T>>> f) =>
            from t in TryAsync(f).ToEither()
            select t.ToList().ToSeq();

        private static UpdateDefinition<TodoItemDto> Updater(TodoItemDto dto) => Builders<TodoItemDto>.Update
            .Set(x => x.Content, dto.Content)
            .Set(x => x.IsDone, dto.IsDone);
    }
}