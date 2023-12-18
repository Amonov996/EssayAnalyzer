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
        
        var expectedFailedUserStorageException = 
            new FailedUserStorageException(sqlException);
        
        // when
        ValueTask<User> addUserTask = this.userService.AddUserAsync(randomUser);

        FailedUserStorageException actualFailedUserStorageException =
            await Assert.ThrowsAsync<FailedUserStorageException>(addUserTask.AsTask);

        // then
        actualFailedUserStorageException.Should().BeEquivalentTo(expectedFailedUserStorageException);
        
        this.loggingBrokerMock.Verify(broker => 
            broker.LogCritical(It.Is(SameExceptionAs(expectedFailedUserStorageException))),
            Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
            broker.InsertUserAsync(randomUser),Times.Never);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}