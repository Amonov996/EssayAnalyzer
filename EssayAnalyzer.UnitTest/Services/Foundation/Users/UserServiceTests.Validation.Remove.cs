using EssayAnalyzer.Api.Models.Foundation.Users;
using EssayAnalyzer.Api.Models.Foundation.Users.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Users;

public partial class UserServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
    {
        // given
        Guid invalidId = Guid.Empty;
        
        InvalidUserException invalidUserException = 
            new InvalidUserException();
        
        invalidUserException.AddData(
            key: nameof(User.Id),
            values: "Id is required");

        var expectedUserDependencyException = 
            new UserDependencyException(invalidUserException);

        // when
        ValueTask<User> removeUserTask = this.userService.RemoveUserByIdAsync(invalidId);

        UserDependencyException actualUserDependencyException =
            await Assert.ThrowsAsync<UserDependencyException>(removeUserTask.AsTask);

        // then
        actualUserDependencyException.Should().BeEquivalentTo(expectedUserDependencyException);
        
        this.loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(expectedUserDependencyException))),
            Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectUserByIdAsync(It.IsAny<Guid>()),Times.Never);
        
        this.storageBrokerMock.Verify(broker => 
            broker.DeleteUserAsync(It.IsAny<User>()),Times.Never);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}