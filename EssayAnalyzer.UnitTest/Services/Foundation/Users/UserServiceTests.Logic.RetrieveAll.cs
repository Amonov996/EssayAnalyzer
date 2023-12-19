using EssayAnalyzer.Api.Models.Foundation.Users;
using FluentAssertions;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Users;

public partial class UserServiceTests
{
    [Fact]
    public async Task ShouldRetrieveAllUsers()
    {
        // given 
        IQueryable<User> randomUsers = CreateRandomUsers();
        IQueryable<User> persistedUsers = randomUsers;
        IQueryable<User> expectedUsers = persistedUsers;

        this.storageBrokerMock.Setup(broker =>
            broker.SelectAllUsers()).Returns(persistedUsers);
        
        // when
        IQueryable<User> actualUsers = this.userService.RetrieveAllUsers();
        
        // then
        actualUsers.Should().BeEquivalentTo(expectedUsers);
        
        this.storageBrokerMock.Verify(broker =>
            broker.SelectAllUsers(),Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}