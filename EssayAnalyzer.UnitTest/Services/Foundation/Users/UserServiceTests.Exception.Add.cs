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
}