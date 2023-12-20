using EssayAnalyzer.Api.Models.Foundation.Users;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Users;

public partial class UserServiceTests
{
    [Fact]
    public async Task ShouldRetrieveUserByIdAsync()
    {
        // given
        Guid randomId = Guid.NewGuid();
        Guid inputId = randomId;
        User randomUser = CreateRandomUser();
        User inputUser = randomUser;
        User persistedUser = inputUser;
        User expectedUser = persistedUser.DeepClone();

        this.storageBrokerMock.Setup(broker =>
            broker.SelectUserByIdAsync(inputId)).ReturnsAsync(persistedUser);

        // when
        User actualUser = await this.userService.RetrieveUserByIdAsync(inputId);

        // when
        actualUser.Should().BeEquivalentTo(expectedUser);
            
        this.storageBrokerMock.Verify(broker =>
            broker.SelectUserByIdAsync(inputId),Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}