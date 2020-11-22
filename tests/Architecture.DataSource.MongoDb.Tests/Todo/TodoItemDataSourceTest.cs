
namespace Architecture.DataSource.MongoDb.Tests.Todo
{
    using System.Threading;

    using Architecture.DataSource.MongoDb.Todo;
    using Architecture.Infrastructure.Todo;

    using MongoDB.Driver;

    using Moq;

    public partial class TodoItemDataSourceTest
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<IMongoClient> _mockMongoClient;
        private readonly Mock<IMongoDatabase> _mockDatabase;
        private readonly Mock<IMongoCollection<TodoItemDto>> _mockCollection;
        private readonly Mock<IAsyncCursor<TodoItemDto>> _mockAsyncCursor;
        private readonly CancellationToken _anyCancellationToken = It.IsAny<CancellationToken>();

        public TodoItemDataSourceTest()
        {
            _mockRepository = new MockRepository(MockBehavior.Default);

            _mockMongoClient = _mockRepository.Create<IMongoClient>();
            _mockDatabase = _mockRepository.Create<IMongoDatabase>();
            _mockCollection = _mockRepository.Create<IMongoCollection<TodoItemDto>>();
            _mockAsyncCursor = _mockRepository.Create<IAsyncCursor<TodoItemDto>>();
        }

        private TodoItemDataSource CreateService()
        {
            _mockDatabase
                .Setup(x => x.GetCollection<TodoItemDto>("items", It.IsAny<MongoCollectionSettings>()))
                .Returns(_mockCollection.Object);

            _mockMongoClient
                .Setup(x => x.GetDatabase("todo", It.IsAny<MongoDatabaseSettings>()))
                .Returns(_mockDatabase.Object);

            return new(_mockMongoClient.Object);
        }
    }
}
