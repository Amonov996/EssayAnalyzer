using EssayAnalyzer.Api.Models.Foundation.Users;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Users;

public partial class UserServiceTests
{
    [Fact]
    public async Task ShouldModifyUserAsync()
    {
        // given 
        User randomUser = CreateRandomUser();
        User inputUser = randomUser;
        User persistedUser = inputUser;
        User updatedUser = inputUser;
        User expectedUser = updatedUser.DeepClone();
        Guid inputUserId = inputUser.Id;

        this.storageBrokerMock.Setup(broker =>
            broker.SelectUserByIdAsync(inputUserId))
            .ReturnsAsync(persistedUser);
        
        this.storageBrokerMock.Setup(broker =>
            broker.UpdateUserAsync(inputUser))
            .ReturnsAsync(updatedUser);

        // when
        User actualModifiedUser = await this.userService
            .ModifyUserAsync(inputUser);

        // then
        actualModifiedUser.Should().BeEquivalentTo(expectedUser);
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectUserByIdAsync(inputUserId),
            Times.Never);
        
        this.storageBrokerMock.Verify(broker => 
            broker.UpdateUserAsync(inputUser),
            Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}