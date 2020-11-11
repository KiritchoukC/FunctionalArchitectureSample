
namespace Architecture.DataSource.MongoDb.Todo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Architecture.Domain.Common.Database;
    using Architecture.Domain.Todo;
    using LanguageExt;
    using LanguageExt.Common;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using static LanguageExt.Prelude;
    using static Architecture.Utils.Constructors.Constructors;

    public class TodoItemDataSource : ITodoItemDataSource
    {
        private readonly IMongoCollection<TodoItemDto> _todoCollection;

        public TodoItemDataSource(IMongoClient mongoClient)
        {
            _todoCollection = mongoClient
                .GetDatabase("todo")
                .GetCollection<TodoItemDto>("items");
        }

        public EitherAsync<DatabaseFailure, Seq<TodoItemDto>> GetAllAsync() =>
            GetAsync(async () => await _todoCollection.FindAsync(x => true))
                .MapLeft(DatabaseFailureCon.Retrieve);

        public EitherAsync<DatabaseFailure, Option<TodoItem>> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public EitherAsync<DatabaseFailure, Unit> AddAsync(TodoItemDto todoItem)
        {
            throw new NotImplementedException();
        }

        public EitherAsync<DatabaseFailure, Unit> UpdateAsync(TodoItemDto todoItem)
        {
            throw new NotImplementedException();
        }

        private EitherAsync<Error, Seq<T>> GetAsync<T>(Func<Task<IAsyncCursor<T>>> f) =>
            from t in TryAsync(f).ToEither()
            select t.ToList().ToSeq();
    }
}