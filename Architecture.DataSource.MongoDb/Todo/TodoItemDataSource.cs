using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Architecture.Domain.Todo;
using LanguageExt;
using LanguageExt.SomeHelp;
using Microsoft.Extensions.Caching.Distributed;
using static LanguageExt.Prelude;

namespace Architecture.DataSource.MongoDb.Todo
{
    public class TodoItemDataSource : ITodoItemDataSource
    {
        public TodoItemDataSource()
        {
        }

        public async Task<Either<TodoFailure, IEnumerable<TodoItem>>> GetAll(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task<Either<TodoFailure, Option<TodoItem>>> GetById(Guid id, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task<Either<TodoFailure, Guid>> Add(TodoItem todoItem, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task<Either<TodoFailure, Unit>> Update(TodoItem todoItem, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}