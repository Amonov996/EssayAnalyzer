using EFxceptions.Models.Exceptions;
using EssayAnalyzer.Api.Models.Foundation.Users;
using EssayAnalyzer.Api.Models.Foundation.Users.Exceptions;
using EssayAnalyzer.Api.Services.Foundation.Users.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Users;

public partial class UserServiceTests
{
    [Fact]
    public async Task ShouldThrowCriticalDependencyExceptionIfSqlErrorOccursAndLogItAsync()
    {
        // given
        User randomUser = CreateRandomUser();
        SqlException sqlException = GetSqlException();

        var failedUserStorageException = 
            new FailedUserStorageException(sqlException);

        var expectedUserDependencyException = 
            new UserDependencyException(failedUserStorageException);

        this.storageBrokerMock.Setup(broker =>
            broker.InsertUserAsync(randomUser)).ThrowsAsync(sqlException);
        
        // when
        ValueTask<User> addUserTask = this.userService.AddUserAsync(randomUser);

        UserDependencyException actualUserDependencyException =
            await Assert.ThrowsAsync<UserDependencyException>(addUserTask.AsTask);

        // then
        actualUserDependencyException.Should().BeEquivalentTo(expectedUserDependencyException);
        
        this.storageBrokerMock.Verify(broker =>
            broker.InsertUserAsync(randomUser), Times.Once);
        
        this.loggingBrokerMock.Verify(broker => 
            broker.LogCritical(It.Is(SameExceptionAs(expectedUserDependencyException))),
            Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowDependencyValidationExceptionOnAddIfUserAlreadyExistsAndLogItAsync()
    {
        // given 
        User randomUser = CreateRandomUser();
        User alreadyExistsUser = randomUser;
        string randomMessage = GetRandomMessage();
        
        var duplicateKeyException = 
            new DuplicateKeyException(randomMessage);

        var alreadyExistsUserException = 
            new AlreadyExistsUserException(duplicateKeyException);

        var expectedUserDependencyValidationException =
            new UserDependencyValidationException(alreadyExistsUserException);

        this.storageBrokerMock.Setup(broker =>
            broker.InsertUserAsync(alreadyExistsUser)).ThrowsAsync(duplicateKeyException);

        // when
        ValueTask<User> addUserTask = this.userService.AddUserAsync(alreadyExistsUser);

        UserDependencyValidationException actualUserDependencyValidationException =
            await Assert.ThrowsAsync<UserDependencyValidationException>(addUserTask.AsTask);

        // then
        actualUserDependencyValidationException.Should()
            .BeEquivalentTo(expectedUserDependencyValidationException);
        
        this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedUserDependencyValidationException))),
            Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
            broker.InsertUserAsync(It.IsAny<User>()),Times.Once);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
    {
        // given
        User randomUser = CreateRandomUser();
        string randomMessage = GetRandomMessage();
        string exceptionMessage = randomMessage;

        var foreignKeyConstraintConflictException =
            new ForeignKeyConstraintConflictException(exceptionMessage);

        InvalidUserRefenerenceException invalidUserRefenerenceException =
            new InvalidUserRefenerenceException(foreignKeyConstraintConflictException);

        var expectedUserDependencyValidationException =
            new UserDependencyValidationException(invalidUserRefenerenceException);

        this.storageBrokerMock.Setup(broker =>
            broker.InsertUserAsync(randomUser)).ThrowsAsync(foreignKeyConstraintConflictException);
        
        // when
        ValueTask<User> addUserTask = this.userService.AddUserAsync(randomUser);

        UserDependencyValidationException actualUserDependencyException =
            await Assert.ThrowsAsync<UserDependencyValidationException>(addUserTask.AsTask);

        // then
        actualUserDependencyException.Should().BeEquivalentTo(expectedUserDependencyValidationException);
        
        this.storageBrokerMock.Verify(broker => 
            broker.InsertUserAsync(It.IsAny<User>()),Times.Once);
        
        this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedUserDependencyValidationException))),
            Times.Once);
    }
}