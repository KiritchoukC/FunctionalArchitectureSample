
namespace Architecture.Infrastructure.Tests.Todo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Architecture.Infrastructure.Todo;

    using Moq;

    public partial class TodoItemRepositoryTest
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<ICache<List<TodoItemDto>>> _mockCache;
        private readonly Mock<ITodoItemDataSource> _mockDataSource;
        private readonly CancellationToken _anyCancellationToken = It.IsAny<CancellationToken>();

        public TodoItemRepositoryTest()
        {
            _mockRepository = new MockRepository(MockBehavior.Default);

            _mockCache = _mockRepository.Create<ICache<List<TodoItemDto>>>();
            _mockDataSource = _mockRepository.Create<ITodoItemDataSource>();
        }

        private ITodoItemRepository CreateService()
        {
            return new TodoItemRepository(
                cache: _mockCache.Object,
                todoItemDataSource: _mockDataSource.Object);
        }
    }
}
