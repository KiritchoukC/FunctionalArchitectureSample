
namespace Architecture.DataSource.MongoDb.Tests.Todo
{
    using System;
    using System.Threading.Tasks;

    using Architecture.DataSource.MongoDb.Todo;
    using Architecture.Domain.Common.Database;
    using Architecture.Infrastructure.Todo;

    using LanguageExt.UnitTesting;

    using MongoDB.Bson;
    using MongoDB.Driver;

    using Moq;

    using Shouldly;

    using Xunit;

    using static LanguageExt.Prelude;

    public partial class TodoItemDataSourceTest
    {
        [Trait("TodoItemDataSource", "Update")]
        [Fact(DisplayName = "With data source returning void should return Right Unit")]
        public async Task Update_WithDataSourceReturningVoid_ShouldReturnRightUnit()
        {
            // Arrange
            var itemToUpdate = new TodoItemDto(Guid.NewGuid(), false, "Test");
            _mockCollection
                .Setup(svc => svc.UpdateOneAsync(
                    It.IsAny<FilterDefinition<TodoItemDto>>(),
                    It.IsAny<UpdateDefinition<TodoItemDto>>(),
                    It.IsAny<UpdateOptions>(),
                    _anyCancellationToken))
                .Returns(Task.FromResult((UpdateResult)new UpdateResult.Acknowledged(2, 2, "123")));

            var dataSource = CreateService();

            // Act
            var actual = dataSource.Update(itemToUpdate);

            // Assert
            await actual.ShouldBeRight();
            await actual.ShouldBeRight(u => u.ShouldBe(unit));
        }

        [Trait("TodoItemDataSource", "Update")]
        [Fact(DisplayName = "With data source throwing exception should return Left Update failure and thrown exception")]
        public async Task Update_WithDataSourceThrowingException_ShouldReturnLeftUpdateFailueAndThrownException()
        {
            // Arrange
            var exception = new Exception("Test exception");
            var itemToUpdate = new TodoItemDto(Guid.NewGuid(), false, "Test");
            _mockCollection
                .Setup(svc => svc.UpdateOneAsync(
                    It.IsAny<FilterDefinition<TodoItemDto>>(), 
                    It.IsAny<UpdateDefinition<TodoItemDto>>(), 
                    It.IsAny<UpdateOptions>(),
                    _anyCancellationToken))
                .Throws(exception);

            var dataSource = CreateService();

            // Act
            var actual = dataSource.Update(itemToUpdate);

            // Assert
            await actual.ShouldBeLeft(failure => failure.ShouldBeOfType<DatabaseFailure.Update>());
            await actual.ShouldBeLeft(failure => failure.Error.Exception.ShouldBeSome());
            await actual.ShouldBeLeft(failure => failure.Error.Exception.ShouldBe(exception));
        }
    }
}
