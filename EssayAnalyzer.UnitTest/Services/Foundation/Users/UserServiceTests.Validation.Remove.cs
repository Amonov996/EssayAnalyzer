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
        
        var invalidUserException = 
            new InvalidUserException();
        
        invalidUserException.AddData(
            key: nameof(User.Id),
            values: "Id is required");

        var expectedUserValidationException = 
            new UserValidationException(invalidUserException);

        // when
        ValueTask<User> removeUserTask = this.userService.RemoveUserByIdAsync(invalidId);

        UserValidationException actualUserValidationException =
            await Assert.ThrowsAsync<UserValidationException>(removeUserTask.AsTask);

        // then
        actualUserValidationException.Should().BeEquivalentTo(expectedUserValidationException);
        
        this.loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
            Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectUserByIdAsync(It.IsAny<Guid>()),Times.Never);
        
        this.storageBrokerMock.Verify(broker => 
            broker.DeleteUserAsync(It.IsAny<User>()),Times.Never);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionOnRemoveIfUserIsNotFoundAndLogItAsync()
    {
        // given
        Guid inputUserId = Guid.NewGuid();
        User noUser = null;

        var notFoundUserException = 
            new NotFoundUserException(inputUserId);

        var expectedUserValidationException = 
            new UserValidationException(notFoundUserException);
        
        // when
        ValueTask<User> removeUserTask = this.userService.RemoveUserByIdAsync(inputUserId);

        UserValidationException actualUserValidationException =
            await Assert.ThrowsAsync<UserValidationException>(removeUserTask.AsTask);

        // then
        actualUserValidationException.Should().BeEquivalentTo(expectedUserValidationException);
        
        this.loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
            Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectUserByIdAsync(It.IsAny<Guid>()), Times.Once);
        
        this.storageBrokerMock.Verify(broker =>
            broker.DeleteUserAsync(It.IsAny<User>()), Times.Never);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}