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
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task ShouldThrowValidationExceptionOnAddIfUserIsInvalidAndLogItAsync(
        string invalidText)
    {
        // given
        User invalidUser = new User()
        {
            FirstName = invalidText,
            LastName = invalidText,
            EmailAddress = invalidText
        };

        var invalidUserException = new InvalidUserException();
        
        invalidUserException.AddData(
            key: nameof(User.Id),
            values: "Id is required");
        
        invalidUserException.AddData(
            key: nameof(User.FirstName),
            values: "Text is required");
        
        invalidUserException.AddData(
            key: nameof(User.LastName),
            values: "Text is required");
        
        invalidUserException.AddData(
            key: nameof(User.EmailAddress),
            values: "Text is required");
        
        var expectedUserValidationException = 
            new UserValidationException(invalidUserException);
        
        // when
        ValueTask<User> addUserTask = this.userService.AddUserAsync(invalidUser);

        UserValidationException actualUserValidationException =
            await Assert.ThrowsAsync<UserValidationException>(addUserTask.AsTask);

        // then
        actualUserValidationException.Should().BeEquivalentTo(expectedUserValidationException);
        
        this.loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
            Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
            broker.InsertUserAsync(invalidUser),Times.Never);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}