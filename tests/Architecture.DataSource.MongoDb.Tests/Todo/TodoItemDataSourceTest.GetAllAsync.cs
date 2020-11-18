
namespace Architecture.DataSource.MongoDb.Tests.Todo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Architecture.DataSource.MongoDb.Todo;
    using Architecture.Domain.Common.Database;

    using LanguageExt.UnitTesting;

    using MongoDB.Driver;

    using Moq;

    using Shouldly;

    using Xunit;

    public partial class TodoItemDataSourceTest
    {

        [Trait("Todo", "GetAllAsync")]
        [Fact(DisplayName = "With data source returning collection should return Right with that collection")]
        public async Task GetAllAsync_WithDataSourceReturningCollection_ShouldReturnRightWithThatCollection()
        {
            // Arrange
            var expected = new List<TodoItemDto> { 
                new TodoItemDto(Guid.NewGuid(), false, "Test 1")
            };

            TestHelper.MockAsyncCursor(_mockAsyncCursor, expected);

            _mockCollection
                .Setup(svc =>
                    svc.FindAsync(
                        It.IsAny<ExpressionFilterDefinition<TodoItemDto>>(),
                        It.IsAny<FindOptions<TodoItemDto, TodoItemDto>>(),
                        _anyCancellationToken))
                .Returns(Task.FromResult(_mockAsyncCursor.Object));

            var dataSource = CreateService();

            // Act
            var actual = dataSource.GetAllAsync();

            // Assert
            await actual.ShouldBeRight();
            await actual.ShouldBeRight(dtos => dtos.ShouldHaveSingleItem());
            await actual.ShouldBeRight(dtos => dtos[0].ShouldBe(expected[0]));
        }

        [Trait("Todo", "GetAllAsync")]
        [Fact(DisplayName = "With data source throwing exception should return Left Retrieve failure and thrown exception")]
        public async Task GetAllAsync_WithDataSourceThrowingException_ShouldReturnLeftRetrieveFailueAndThrownException()
        {
            // Arrange
            var exception = new Exception("Test exception");
            _mockCollection
                .Setup(svc =>
                    svc.FindAsync(
                        It.IsAny<ExpressionFilterDefinition<TodoItemDto>>(),
                        It.IsAny<FindOptions<TodoItemDto, TodoItemDto>>(),
                        _anyCancellationToken))
                .Throws(exception);

            var dataSource = CreateService();

            // Act
            var actual = dataSource.GetAllAsync();

            // Assert
            await actual.ShouldBeLeft(failure => failure.ShouldBeOfType<DatabaseFailure.Retrieve>());
            await actual.ShouldBeLeft(failure => failure.Error.Exception.ShouldBeSome());
            await actual.ShouldBeLeft(failure => failure.Error.Exception.ShouldBe(exception));
        }
    }
}
