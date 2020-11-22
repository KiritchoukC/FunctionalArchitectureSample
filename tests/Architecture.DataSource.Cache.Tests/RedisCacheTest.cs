namespace Architecture.DataSource.Cache.Tests
{
    using System.Collections.Generic;
    using System.Threading;

    using Architecture.Infrastructure.Todo;

    using Microsoft.Extensions.Caching.Distributed;

    using Moq;
    public partial class RedisCacheTest
    {
        private const string _cacheKey = "TestCacheKey";

        private readonly MockRepository _mockRepository;
        private readonly Mock<IDistributedCache> _mockCache;
        private readonly CancellationToken _anyCancellationToken = It.IsAny<CancellationToken>();

        public RedisCacheTest()
        {
            _mockRepository = new MockRepository(MockBehavior.Default);

            _mockCache = _mockRepository.Create<IDistributedCache>();
        }

        private RedisCache<List<TodoItemDto>> CreateService()
        {
            return new(_mockCache.Object);
        }
    }
}
