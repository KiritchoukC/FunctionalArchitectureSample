
namespace Architecture.DataSource.MongoDb.Tests.Todo
{
    using System;
    using System.Threading.Tasks;

    using Architecture.DataSource.MongoDb.Todo;
    using Architecture.Domain.Common.Database;
    using Architecture.Infrastructure.Todo;

    using LanguageExt.UnitTesting;

    using MongoDB.Driver;

    using Moq;

    using Shouldly;

    using Xunit;

    using static LanguageExt.Prelude;

    public partial class TodoItemDataSourceTest
    {
        [Trait("TodoItemDataSource", "Add")]
        [Fact(DisplayName = "With data source returning void should return Right Unit")]
        public async Task Add_WithDataSourceReturningVoid_ShouldReturnRightUnit()
        {
            // Arrange
            var itemToAdd = new TodoItemDto(Guid.NewGuid(), false, "Test");

            _mockCollection
                .Setup(svc => svc.InsertOneAsync(itemToAdd, It.IsAny<InsertOneOptions>(), _anyCancellationToken))
                .Returns(Task.CompletedTask);

            var dataSource = CreateService();

            // Act
            var actual = dataSource.Add(itemToAdd);

            // Assert
            await actual.ShouldBeRight();
            await actual.ShouldBeRight(u => u.ShouldBe(unit));
        }

        [Trait("TodoItemDataSource", "Add")]
        [Fact(DisplayName = "With data source throwing exception should return Left Insert failure and thrown exception")]
        public async Task Add_WithDataSourceThrowingException_ShouldReturnLeftInsertFailueAndThrownException()
        {
            // Arrange
            var exception = new Exception("Test exception");
            var itemToAdd = new TodoItemDto(Guid.NewGuid(), false, "Test");
            _mockCollection
                .Setup(svc => svc.InsertOneAsync(itemToAdd, It.IsAny<InsertOneOptions>(), _anyCancellationToken))
                .Throws(exception);

            var dataSource = CreateService();

            // Act
            var actual = dataSource.Add(itemToAdd);

            // Assert
            await actual.ShouldBeLeft(failure => failure.ShouldBeOfType<DatabaseFailure.Insert>());
            await actual.ShouldBeLeft(failure => failure.Error.Exception.ShouldBeSome());
            await actual.ShouldBeLeft(failure => failure.Error.Exception.ShouldBe(exception));
        }
    }
}
