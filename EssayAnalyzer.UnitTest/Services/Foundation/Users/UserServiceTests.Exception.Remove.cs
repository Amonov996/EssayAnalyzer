using EssayAnalyzer.Api.Models.Foundation.Users;
using EssayAnalyzer.Api.Models.Foundation.Users.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Users;

public partial class UserServiceTests
{
    [Fact]
    public async Task ShouldThrowCriticalValidationExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
    {
        // given
        Guid someUserId = Guid.NewGuid();
        SqlException sqlException = GetSqlException();

        var failedUserStorageException = 
            new FailedUserStorageException(sqlException);

        var expectedUserDependencyException =
            new UserDependencyException(failedUserStorageException);

        this.storageBrokerMock.Setup(broker =>
            broker.SelectUserByIdAsync(someUserId)).Throws(sqlException);
        
        // when
        ValueTask<User> removeUserTask = this.userService.RemoveUserByIdAsync(someUserId);

        UserDependencyException actualUserDependencyException =
            await Assert.ThrowsAsync<UserDependencyException>(removeUserTask.AsTask);

        // then
        actualUserDependencyException.Should().BeEquivalentTo(expectedUserDependencyException);
        
        this.loggingBrokerMock.Verify(broker => 
            broker.LogCritical(It.Is(SameExceptionAs(expectedUserDependencyException))),
            Times.Once);
        
        this.storageBrokerMock.Verify(broker =>
            broker.SelectUserByIdAsync(It.IsAny<Guid>()),Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
            broker.DeleteUserAsync(It.IsAny<User>()), Times.Never);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnRemoveIfServiceErrorOccursAndLogItAsync()
    {
        // given
        Guid someUserId = Guid.NewGuid();
        Exception serviceException = new Exception();

        var failedUserServiceException = 
            new FailedUserServiceException(serviceException);

        var expectedUserServiceException = 
            new UserServiceException(failedUserServiceException);

        this.storageBrokerMock.Setup(broker =>
            broker.SelectUserByIdAsync(It.IsAny<Guid>())).Throws(serviceException);
        
        // when
        ValueTask<User> removeUserTask = this.userService.RemoveUserByIdAsync(someUserId);

        UserServiceException actualUserServiceException =
            await Assert.ThrowsAsync<UserServiceException>(removeUserTask.AsTask);

        // then
        actualUserServiceException.Should().BeEquivalentTo(expectedUserServiceException);
        
        this.loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(expectedUserServiceException))),
            Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectUserByIdAsync(It.IsAny<Guid>()), Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
            broker.DeleteUserAsync(It.IsAny<User>()), Times.Never);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}