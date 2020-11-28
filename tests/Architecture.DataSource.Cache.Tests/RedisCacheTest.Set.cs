
namespace Architecture.DataSource.Cache.Tests
{
    using System;
    using System.Text;
    using System.Threading.Tasks;

    using Architecture.Domain.Common.Cache;
    using Architecture.Infrastructure.Todo;

    using LanguageExt.UnitTesting;

    using static LanguageExt.Prelude;

    using Shouldly;

    using Xunit;
    using Microsoft.Extensions.Caching.Distributed;
    using Moq;

    public partial class RedisCacheTest
    {
        [Trait("Cache", "Set")]
        [Fact(DisplayName = "With cache throwing exception should return Left(CacheFailure) with thrown exception")]
        public async Task Set_WithCacheThrowingException_ShouldReturnLeftCacheFailureWithThrownException()
        {
            // Arrange
            var service = CreateService();
            var exception = new Exception("Something wrong happend in cache");
            var items = List(new TodoItemDto(Guid.NewGuid(), true, "Test content")).ToList();
            var bytes = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(items));

            _mockCache
                .Setup(x => x.SetAsync(_cacheKey, bytes, It.IsAny<DistributedCacheEntryOptions>(), _anyCancellationToken))
                .Throws(exception);

            // Act
            var actual = service.Set(_cacheKey, items);

            // Assert
            await actual.ShouldBeLeft();
            await actual.ShouldBeLeft(failure => failure.ShouldBeOfType<CacheFailure.Insert>());
            await actual.ShouldBeLeft(failure => failure.Error.Exception.ShouldBeSome());
            await actual.ShouldBeLeft(failure => failure.Error.Exception.ShouldBeSome(e => e.ShouldBe(exception)));
        }

        [Trait("Cache", "Set")]
        [Fact(DisplayName = "With cache returning null should return Right(None)")]
        public async Task Set_WithCacheReturningTask_ShouldReturnRightUnit()
        {
            // Arrange
            var service = CreateService();
            var items = List(new TodoItemDto(Guid.NewGuid(), true, "Test content")).ToList();
            var bytes = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(items));

            _mockCache
                .Setup(x => x.SetAsync(_cacheKey, bytes, It.IsAny<DistributedCacheEntryOptions>(), _anyCancellationToken))
                .Returns(Task.CompletedTask);

            // Act
            var actual = service.Set(_cacheKey, items);

            // Assert
            await actual.ShouldBeRight();
            await actual.ShouldBeRight(cache => cache.ShouldBe(unit));
        }
    }
}
