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
}