using EssayAnalyzer.Api.Models.Foundation.Users;
using EssayAnalyzer.Api.Models.Foundation.Users.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Users;

public partial class UserServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnModifyIfUserIsNullAndLogItAsync()
    {
        // given
        User nullUser = null;
        var nullUserException = new NullUserException();

        var expectedUserValidationException = 
            new UserValidationException(nullUserException);
        
        // when
        ValueTask<User> modifyUserTask = this.userService.ModifyUserAsync(nullUser);

        UserValidationException actualUserValidationException =
            await Assert.ThrowsAsync<UserValidationException>(modifyUserTask.AsTask);

        // then
        actualUserValidationException.Should().BeEquivalentTo(expectedUserValidationException);
        
        this.loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
            Times.Once);
        
        this.storageBrokerMock.Verify(broker =>
            broker.SelectUserByIdAsync(It.IsAny<Guid>()),
            Times.Never);
        
        this.storageBrokerMock.Verify(broker => 
            broker.UpdateUserAsync(It.IsAny<User>()),Times.Never);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}