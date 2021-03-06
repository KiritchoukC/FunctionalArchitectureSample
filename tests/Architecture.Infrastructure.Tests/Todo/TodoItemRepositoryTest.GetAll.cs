﻿
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
        [Trait("TodoItemRepository", "GetAll")]
        [Fact(DisplayName = "With cache returning Left should return Left(CacheFailure)")]
        public async void GetAll_WithCacheReturningLeft_ShouldReturnLeftCacheFailure()
        {
            // Arrange
            var service = CreateService();

            var cacheFailure = CacheFailureCon.Fetch(Error.New("Cache error"));
            Either<CacheFailure, Option<List<TodoItemDto>>> cacheResult = Left(cacheFailure);

            _mockCache
                .Setup(c => c.Get(It.IsAny<string>()))
                .Returns(cacheResult.ToAsync());

            // Act
            var actual = service.GetAll();

            // Assert
            await actual.ShouldBeLeft();
            await actual.ShouldBeLeft(f => f.ShouldBeOfType<TodoFailure.Cache>());
            await actual.ShouldBeLeft(f => (f as TodoFailure.Cache).Failure.ShouldBe(cacheFailure));
        }

        [Trait("TodoItemRepository", "GetAll")]
        [Fact(DisplayName = "With cache returning Right(Some(items)) should return Right(items)")]
        public async void GetAll_WithCacheReturningSomeItems_ShouldReturnThoseItems()
        {
            // Arrange
            var service = CreateService();

            var items = List(
                new TodoItemDto(Guid.NewGuid(), false, "test content 1"),
                new TodoItemDto(Guid.NewGuid(), false, "test content 2")).ToList();

            _mockCache
                .Setup(c => c.Get(It.IsAny<string>()))
                .Returns(Some(items));

            // Act
            var actual = service.GetAll();

            // Assert
            await actual.ShouldBeRight();
            await actual.ShouldBeRight(r => r.ShouldContain(x => x.Id.Value == items[0].Id));
            await actual.ShouldBeRight(r => r.ShouldContain(x => x.Id.Value == items[1].Id));
        }

        [Trait("TodoItemRepository", "GetAll")]
        [Fact(DisplayName = "With cache returning Right(None) should call datasource")]
        public void GetAll_WithCacheReturningNone_ShouldCallDataSource()
        {
            // Arrange
            var service = CreateService();

            var items = List(
                new TodoItemDto(Guid.NewGuid(), false, "test content 1"),
                new TodoItemDto(Guid.NewGuid(), false, "test content 2")).ToSeq();

            var cacheResult = Right<CacheFailure, Option<List<TodoItemDto>>>(None);

            _mockCache
                .Setup(c => c.Get(It.IsAny<string>()))
                .Returns(cacheResult.ToAsync())
                .Verifiable();

            _mockCache
                .Setup(c => c.Set(It.IsAny<string>(), It.IsAny<List<TodoItemDto>>()))
                .Returns(unit)
                .Verifiable();

            _mockDataSource
                .Setup(ds => ds.GetAll())
                .Returns(items)
                .Verifiable();

            // Act
            var actual = service.GetAll();

            // Assert
            _mockRepository.Verify();
        }

        [Trait("TodoItemRepository", "GetAll")]
        [Fact(DisplayName = "With cache returning Right(None) should return DataSource items")]
        public async void GetAll_WithCacheReturningNone_ShouldReturnDataSourceItems()
        {
            // Arrange
            var service = CreateService();

            var items = List(
                new TodoItemDto(Guid.NewGuid(), false, "test content 1"),
                new TodoItemDto(Guid.NewGuid(), false, "test content 2")).ToSeq();

            var cacheResult = Right<CacheFailure, Option<List<TodoItemDto>>>(None);

            _mockCache
                .Setup(c => c.Get(It.IsAny<string>()))
                .Returns(cacheResult.ToAsync())
                .Verifiable();

            _mockCache
                .Setup(c => c.Set(It.IsAny<string>(), It.IsAny<List<TodoItemDto>>()))
                .Returns(unit)
                .Verifiable();

            _mockDataSource
                .Setup(ds => ds.GetAll())
                .Returns(items)
                .Verifiable();

            // Act
            var actual = service.GetAll();

            // Assert
            await actual.ShouldBeRight();
            await actual.ShouldBeRight(r => r.ShouldContain(x => x.Id.Value == items[0].Id));
            await actual.ShouldBeRight(r => r.ShouldContain(x => x.Id.Value == items[1].Id));
        }

        [Trait("TodoItemRepository", "GetAll")]
        [Fact(DisplayName = "With cache returning None and cache Set returning Left(CacheFailure) should return Left(TodoFailure.Cache)")]
        public async void GetAll_WithCacheReturningNoneAndCacheSetReturningLeftCacheFailure_ShouldReturnLeftTodoFailureCache()
        {
            // Arrange
            var service = CreateService();

            var items = List(
                new TodoItemDto(Guid.NewGuid(), false, "test content 1"),
                new TodoItemDto(Guid.NewGuid(), false, "test content 2")).ToSeq();

            var cacheResult = Right<CacheFailure, Option<List<TodoItemDto>>>(None);
            var cacheSetFailure = CacheFailureCon.Insert(Error.New("Cache set error"));
            var cacheSetResult = Left<CacheFailure, Unit>(cacheSetFailure);

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

            // Act
            var actual = service.GetAll();

            // Assert
            await actual.ShouldBeLeft();
            await actual.ShouldBeLeft(f => f.ShouldBeOfType<TodoFailure.Cache>());
            await actual.ShouldBeLeft(f => (f as TodoFailure.Cache).Failure.ShouldBe(cacheSetFailure));
        }

        [Trait("TodoItemRepository", "GetAll")]
        [Fact(DisplayName = "With cache returning None and datasource returning Left(DatabaseFailure) should return Left(TodoFailure.Database)")]
        public async void GetAll_WithCacheReturningNoneAndDataSourceReturningLeftDatabaseFailure_ShouldReturnLeftTodoFailureDatabase()
        {
            // Arrange
            var service = CreateService();

            var items = List(
                new TodoItemDto(Guid.NewGuid(), false, "test content 1"),
                new TodoItemDto(Guid.NewGuid(), false, "test content 2")).ToSeq();

            var cacheResult = Right<CacheFailure, Option<List<TodoItemDto>>>(None);
            var dataSourceFailure = DatabaseFailureCon.Retrieve(Error.New("Database retrieve error"));
            var dataSourceResult = Left<DatabaseFailure, Seq<TodoItemDto>>(dataSourceFailure);

            _mockCache
                .Setup(c => c.Get(It.IsAny<string>()))
                .Returns(cacheResult.ToAsync())
                .Verifiable();

            _mockCache
                .Setup(c => c.Set(It.IsAny<string>(), It.IsAny<List<TodoItemDto>>()))
                .Returns(unit)
                .Verifiable();

            _mockDataSource
                .Setup(ds => ds.GetAll())
                .Returns(dataSourceResult.ToAsync())
                .Verifiable();

            // Act
            var actual = service.GetAll();

            // Assert
            await actual.ShouldBeLeft();
            await actual.ShouldBeLeft(f => f.ShouldBeOfType<TodoFailure.Database>());
            await actual.ShouldBeLeft(f => (f as TodoFailure.Database).Failure.ShouldBe(dataSourceFailure));
        }
    }
}
