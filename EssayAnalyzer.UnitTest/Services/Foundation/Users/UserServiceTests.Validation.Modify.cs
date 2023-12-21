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
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task ShouldThrowValidationExceptionOnModifyIfUserIsInvalidAndLogItAsync(
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
        ValueTask<User> addUserTask = this.userService.ModifyUserAsync(invalidUser);

        UserValidationException actualUserValidationException =
            await Assert.ThrowsAsync<UserValidationException>(addUserTask.AsTask);

        // then
        actualUserValidationException.Should().BeEquivalentTo(expectedUserValidationException);
        
        this.loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
            Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
            broker.UpdateUserAsync(invalidUser),Times.Never);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task ShouldThrowNotFoundExceptionOnModifyIfUserIsNotExistsAndLogItAsync()
    {
        // given
        User randomUser = CreateRandomUser();
        User nonExistsUser = randomUser;
        User nullUser = null;

        var notFoundUserException = 
            new NotFoundUserException(nonExistsUser.Id);

        var expectedUserValidationException = 
            new UserValidationException(notFoundUserException);

        this.storageBrokerMock.Setup(broker =>
            broker.SelectUserByIdAsync(nonExistsUser.Id)).ReturnsAsync(nullUser);
        
        // when
        ValueTask<User> modifyUserTask = this.userService.ModifyUserAsync(nonExistsUser);

        UserValidationException actualUserValidationException =
            await Assert.ThrowsAsync<UserValidationException>(modifyUserTask.AsTask);

        // given
        this.storageBrokerMock.Verify(broker => 
            broker.SelectUserByIdAsync(nonExistsUser.Id), Times.Once);
        
        this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
            Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}