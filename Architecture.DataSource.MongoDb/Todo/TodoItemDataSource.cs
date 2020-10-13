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

namespace Architecture.DataSource.MongoDb.Todo
{
    public class TodoItemDataSource : ITodoItemDataSource
    {
        private readonly IMongoCollection<TodoItemDto> _todoCollection;

        public TodoItemDataSource(IMongoClient mongoClient)
        {
            _todoCollection = mongoClient
                .GetDatabase("todo")
                .GetCollection<TodoItemDto>("items");
        }

        public Either<DatabaseFailure, IEnumerable<TodoItemDto>> GetAll()
        {
            return Try(() => _todoCollection.Find(x => true).ToList())
                .ToEither()
                .MapLeft(DatabaseFailureCon.Retrieve)
                .Map(items => items.AsEnumerable())
                .Map(items => items ?? Enumerable.Empty<TodoItemDto>());
        }

        public Either<DatabaseFailure, Option<TodoItem>> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Either<DatabaseFailure, Unit> Add(TodoItemDto todoItem)
        {
            throw new NotImplementedException();
        }

        public Either<DatabaseFailure, Unit> Update(TodoItemDto todoItem)
        {
            throw new NotImplementedException();
        }
    }
}