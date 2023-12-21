using EFxceptions.Models.Exceptions;
using EssayAnalyzer.Api.Models.Foundation.Users;
using EssayAnalyzer.Api.Models.Foundation.Users.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Users;

public partial class UserServiceTests
{
    [Fact]
    public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
    {
        // given
        User randomUser = CreateRandomUser();
        SqlException sqlException = GetSqlException();

        var failedUserStorageException = 
            new FailedUserStorageException(sqlException);

        var expectedUserDependencyException =
            new UserDependencyException(failedUserStorageException);

        this.storageBrokerMock.Setup(broker =>
            broker.SelectUserByIdAsync(randomUser.Id)).ReturnsAsync(randomUser);
        
        this.storageBrokerMock.Setup(broker =>
            broker.UpdateUserAsync(randomUser)).Throws(sqlException);
        
        // when
        ValueTask<User> modifyUserTask = this.userService.ModifyUserAsync(randomUser);

        UserDependencyException actualUserDependencyException =
            await Assert.ThrowsAsync<UserDependencyException>(modifyUserTask.AsTask);

        // then
        actualUserDependencyException.Should().BeEquivalentTo(expectedUserDependencyException);
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectUserByIdAsync(randomUser.Id), Times.Once);
        
        this.storageBrokerMock.Verify(broker =>
            broker.UpdateUserAsync(randomUser), Times.Once);
        
        this.loggingBrokerMock.Verify(broker => 
                broker.LogCritical(It.Is(SameExceptionAs(expectedUserDependencyException))),
            Times.Once);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
    {
        // given 
        User randomUser = CreateRandomUser();
        User foreignKeyConflictedUser = randomUser;
        string randomMessage = GetRandomMessage();
        string exceptionMessage = randomMessage;

        var foreignKeyConstraintConflictException =
            new ForeignKeyConstraintConflictException(exceptionMessage);

        var invalidUserReferenceException =
            new InvalidUserRefenerenceException(foreignKeyConstraintConflictException);

        var expectedUserDependencyValidationException =
            new UserDependencyValidationException(invalidUserReferenceException);

        this.storageBrokerMock.Setup(broker =>
            broker.SelectUserByIdAsync(foreignKeyConflictedUser.Id))
            .ReturnsAsync(foreignKeyConflictedUser);
        
        this.storageBrokerMock.Setup(broker =>
            broker.UpdateUserAsync(foreignKeyConflictedUser))
            .Throws(foreignKeyConstraintConflictException);
        
        // when
        ValueTask<User> modifyUserTask = this.userService.ModifyUserAsync(foreignKeyConflictedUser);

        UserDependencyValidationException actualUserDependencyValidationException =
            await Assert.ThrowsAsync<UserDependencyValidationException>(modifyUserTask.AsTask);

        // then
        actualUserDependencyValidationException.Should()
            .BeEquivalentTo(expectedUserDependencyValidationException);
        
        this.storageBrokerMock.Verify(broker =>
            broker.SelectUserByIdAsync(foreignKeyConflictedUser.Id), Times.Once);
        
        this.storageBrokerMock.Verify(broker =>
            broker.UpdateUserAsync(foreignKeyConflictedUser), Times.Once);
        
        this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedUserDependencyValidationException))),
            Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
    {
        // given
        User randomUser = CreateRandomUser();
        DbUpdateException dbUpdateException = new DbUpdateException();

        var failedUserStorageException = 
            new FailedUserStorageException(dbUpdateException);

        var expectedUserDependencyException =
            new UserDependencyException(failedUserStorageException);

        this.storageBrokerMock.Setup(broker =>
            broker.SelectUserByIdAsync(randomUser.Id)).ReturnsAsync(randomUser);

        this.storageBrokerMock.Setup(broker =>
            broker.UpdateUserAsync(randomUser)).Throws(dbUpdateException);

        // when
        ValueTask<User> modifyUserTask = this.userService.ModifyUserAsync(randomUser);

        UserDependencyException actualUserDependencyException =
            await Assert.ThrowsAsync<UserDependencyException>(modifyUserTask.AsTask);

        // then
        actualUserDependencyException.Should().BeEquivalentTo(expectedUserDependencyException);
        
        this.storageBrokerMock.Verify(broker =>
            broker.SelectUserByIdAsync(randomUser.Id), Times.Once);
        
        this.storageBrokerMock.Verify(broker =>
            broker.UpdateUserAsync(randomUser), Times.Once);
        
        this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedUserDependencyException))),
            Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}