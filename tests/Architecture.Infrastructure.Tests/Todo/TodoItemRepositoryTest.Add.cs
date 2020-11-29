
namespace Architecture.Infrastructure.Tests.Todo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Architecture.Domain.Common.Cache;
    using Architecture.Domain.Common.Database;
    using Architecture.Domain.Todo;
    using Architecture.Infrastructure.Todo;

    using LanguageExt;
    using LanguageExt.Common;
    using LanguageExt.UnitTesting;

    using Moq;

    using Shouldly;

    using Xunit;

    using static Architecture.Utils.Constructors;
    using static LanguageExt.Prelude;

    public partial class TodoItemRepositoryTest
    {
        [Trait("TodoItemRepository", "Add")]
        [Fact(DisplayName = "With DataSource.Add returning Left(DatabaseFailure) should return Left(TodoFailure.Database)")]
        public async void Add_WithDataSourceAddReturningLeftDatabaseFailure_ShouldReturnLeftTodoFailureCacheDatabase()
        {
            // Arrange
            var service = CreateService();

            var itemToAdd = TodoItem.New(Guid.NewGuid(), false, "test content").SuccessAsEnumerable()[0];

            var items = List(
                new TodoItemDto(Guid.NewGuid(), false, "test content 1"),
                new TodoItemDto(Guid.NewGuid(), true, "test content 2")).ToSeq();

            var cacheResult = Right<CacheFailure, Option<List<TodoItemDto>>>(None);
            var cacheSetResult = Right<CacheFailure, Unit>(unit);
            var dataSourceFailure = DatabaseFailureCon.Insert(Error.New("Datbase insert error"));
            var dataSourceAddResult = Left<DatabaseFailure, Unit>(dataSourceFailure);

            _mockCache
                .Setup(c => c.Get(It.IsAny<string>()))
                .Returns(cacheResult.ToAsync())
                .Verifiable();

            _mockCache
                .Setup(c => c.Set(It.IsAny<string>(), It.IsAny<List<TodoItemDto>>()))
                .Returns(cacheSetResult.ToAsync())
                .Verifiable();

            _mockDataSource
                .Setup(ds => ds.GetAll())
                .Returns(items)
                .Verifiable();

            _mockDataSource
                .Setup(ds => ds.Add(It.IsAny<TodoItemDto>()))
                .Returns(dataSourceAddResult.ToAsync());

            // Act
            var actual = service.Add(itemToAdd);

            // Assert
            await actual.ShouldBeLeft();
            await actual.ShouldBeLeft(f => f.ShouldBeOfType<TodoFailure.Database>());
            await actual.ShouldBeLeft(f => (f as TodoFailure.Database).Failure.ShouldBe(dataSourceFailure));
        }
        [Trait("TodoItemRepository", "Add")]
        [Fact(DisplayName = "With everything Right should return Right(Unit)")]
        public async void Add_WithEverythingRight_ShouldReturnRightUnit()
        {
            // Arrange
            var service = CreateService();

            var itemToAdd = TodoItem.New(Guid.NewGuid(), false, "test content").SuccessAsEnumerable()[0];

            var items = List(
                new TodoItemDto(Guid.NewGuid(), false, "test content 1"),
                new TodoItemDto(Guid.NewGuid(), true, "test content 2")).ToSeq();

            var cacheResult = Right<CacheFailure, Option<List<TodoItemDto>>>(None);
            var cacheSetResult = Right<CacheFailure, Unit>(unit);
            var dataSourceAddResult = Right<DatabaseFailure, Unit>(unit);

            _mockCache
                .Setup(c => c.Get(It.IsAny<string>()))
                .Returns(cacheResult.ToAsync())
                .Verifiable();

            _mockCache
                .Setup(c => c.Set(It.IsAny<string>(), It.IsAny<List<TodoItemDto>>()))
                .Returns(cacheSetResult.ToAsync())
                .Verifiable();

            _mockDataSource
                .Setup(ds => ds.GetAll())
                .Returns(items)
                .Verifiable();

            _mockDataSource
                .Setup(ds => ds.Add(It.IsAny<TodoItemDto>()))
                .Returns(dataSourceAddResult.ToAsync());

            // Act
            var actual = service.Add(itemToAdd);

            // Assert
            await actual.ShouldBeRight();
            await actual.ShouldBeRight(x => x.ShouldBe(unit));
        }
    }
}
