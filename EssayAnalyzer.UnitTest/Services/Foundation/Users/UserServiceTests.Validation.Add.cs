using EssayAnalyzer.Api.Models.Foundation.Users;
using EssayAnalyzer.Api.Models.Foundation.Users.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Users;

public partial class UserServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnAddIfUserIsNullAndLogItAsync()
    {
        // given
        User nullUser = null;
        var nullUserException = new NullUserException();
        
        var expectedUserValidationException = 
            new UserValidationException(nullUserException);

        // when
        ValueTask<User> addUserTask = this.userService.AddUserAsync(nullUser);

        UserValidationException actualUserValidationException =
            await Assert.ThrowsAsync<UserValidationException>(addUserTask.AsTask);

        // then
        actualUserValidationException.Should()
            .BeEquivalentTo(expectedUserValidationException);
        
        this.loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(
                SameExceptionAs(expectedUserValidationException))),
            Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
            broker.InsertUserAsync(nullUser),Times.Never);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}