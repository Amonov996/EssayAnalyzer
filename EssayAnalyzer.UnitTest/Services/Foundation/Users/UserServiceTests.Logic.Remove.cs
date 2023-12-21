using EssayAnalyzer.Api.Models.Foundation.Users;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Users;

public partial class UserServiceTests
{
    [Fact]
    public async Task ShouldRemoveUserAsync()
    {
        // given
        Guid randomId = Guid.NewGuid();
        Guid inputId = randomId;
        User randomUser = CreateRandomUser();
        User persistedUser = randomUser;
        User expectedUserInput = persistedUser;
        User deletedUser = expectedUserInput;
        User expectedUser = deletedUser.DeepClone();

        this.storageBrokerMock.Setup(broker =>
            broker.SelectUserByIdAsync(inputId))
            .ReturnsAsync(persistedUser);

        this.storageBrokerMock.Setup(broker =>
            broker.DeleteUserAsync(expectedUserInput))
            .ReturnsAsync(deletedUser);
        
        // when
        User actualUser = await this.userService
            .RemoveUserByIdAsync(inputId);

        // then
        actualUser.Should().BeEquivalentTo(expectedUser);
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectUserByIdAsync(inputId),Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
            broker.DeleteUserAsync(persistedUser),Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}