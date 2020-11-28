
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

    public partial class RedisCacheTest
    {
        [Trait("Cache", "Get")]
        [Fact(DisplayName = "With cache throwing exception should return Left(CacheFailure) with thrown exception")]
        public async Task Get_WithCacheThrowingException_ShouldReturnLeftCacheFailureWithThrownException()
        {
            // Arrange
            var service = CreateService();
            var exception = new Exception("Something wrong happend in cache");

            _mockCache
                .Setup(x => x.GetAsync(_cacheKey, _anyCancellationToken))
                .Throws(exception);

            // Act
            var actual = service.Get(_cacheKey);

            // Assert
            await actual.ShouldBeLeft();
            await actual.ShouldBeLeft(failure => failure.ShouldBeOfType<CacheFailure.Fetch>());
            await actual.ShouldBeLeft(failure => failure.Error.Exception.ShouldBeSome());
            await actual.ShouldBeLeft(failure => failure.Error.Exception.ShouldBeSome(e => e.ShouldBe(exception)));
        }

        [Trait("Cache", "Get")]
        [Fact(DisplayName = "With cache returning null should return Right(None)")]
        public async Task Get_WithCacheReturningNull_ShouldReturnRightNone()
        {
            // Arrange
            var service = CreateService();

            _mockCache
                .Setup(x => x.GetAsync(_cacheKey, _anyCancellationToken))
                .Returns(Task.FromResult<byte[]>(null));

            // Act
            var actual = service.Get(_cacheKey);

            // Assert
            await actual.ShouldBeRight();
            await actual.ShouldBeRight(cache => cache.ShouldBeNone());
        }

        [Trait("Cache", "Get")]
        [Fact(DisplayName = "With cache returning something should return Right(Some(object))")]
        public async Task Get_WithCacheReturningSomething_ShouldReturnRightSomeObject()
        {
            // Arrange
            var service = CreateService();
            var expected = List(new TodoItemDto(Guid.NewGuid(), true, "Test content")).ToList();
            var bytes = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(expected));

            _mockCache
                .Setup(x => x.GetAsync(_cacheKey, _anyCancellationToken))
                .Returns(Task.FromResult(bytes));

            // Act
            var actual = service.Get(_cacheKey);

            // Assert
            await actual.ShouldBeRight();
            await actual.ShouldBeRight(cache => cache.ShouldBeSome());
            await actual.ShouldBeRight(cache => cache.ShouldBeSome(s => s.ShouldHaveSingleItem()));
            await actual.ShouldBeRight(cache => cache.ShouldBeSome(s => s[0].ShouldBeEquivalentTo(expected[0])));
        }
    }
}
