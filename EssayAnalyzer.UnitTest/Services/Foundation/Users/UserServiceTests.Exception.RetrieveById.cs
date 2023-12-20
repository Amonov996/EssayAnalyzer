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
    public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
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
        ValueTask<User> retrieveUserByIdTask = userService.RetrieveUserByIdAsync(someUserId);

        UserDependencyException actualUserDependencyException =
            await Assert.ThrowsAsync<UserDependencyException>(retrieveUserByIdTask.AsTask);

        // then
        actualUserDependencyException.Should().BeEquivalentTo(expectedUserDependencyException);
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectUserByIdAsync(someUserId), Times.Once);
        
        this.loggingBrokerMock.Verify(broker => 
            broker.LogCritical(It.Is(SameExceptionAs(expectedUserDependencyException))),
            Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
    {
        // given 
        Guid someUserId = Guid.NewGuid();
        Exception serviceException = new Exception();

        var failedUserServiceException = 
            new FailedUserServiceException(serviceException);

        var expectedUserServiceException = 
            new UserServiceException(failedUserServiceException);

        this.storageBrokerMock.Setup(broker =>
            broker.SelectUserByIdAsync(someUserId)).Throws(serviceException);
        
        // when
        ValueTask<User> retrieveUserByIdTask = 
            this.userService.RetrieveUserByIdAsync(someUserId);

        UserServiceException actualUserServiceException =
            await Assert.ThrowsAsync<UserServiceException>(retrieveUserByIdTask.AsTask);

        // then
        actualUserServiceException.Should().BeEquivalentTo(expectedUserServiceException);
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectUserByIdAsync(someUserId),Times.Once);
        
        this.loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(expectedUserServiceException))),
            Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}