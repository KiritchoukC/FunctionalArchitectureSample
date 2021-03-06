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
        private static TodoId GetTodoId(Guid? guid = null) => TodoId.New(guid ?? Guid.NewGuid()).SuccessAsEnumerable()[0];

        [Trait("TodoItemRepository", "GetById")]
        [Fact(DisplayName = "With not found item should return Right(None)")]
        public async void GetById_WithNotFoundItem_ShouldReturnRightNone()
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
            var actual = service.GetById(GetTodoId());

            // Assert
            await actual.ShouldBeRight();
            await actual.ShouldBeRight(r => r.ShouldBeNone());
        }

        [Trait("TodoItemRepository", "GetById")]
        [Fact(DisplayName = "With found item should return Right(Some)")]
        public async void GetById_WithFoundItem_ShouldReturnRightSome()
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
            var actual = service.GetById(GetTodoId(items[0].Id));

            // Assert
            await actual.ShouldBeRight();
            await actual.ShouldBeRight(r => r.ShouldBeSome());
        }
    }
}
