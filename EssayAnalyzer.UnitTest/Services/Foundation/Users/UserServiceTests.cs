using EssayAnalyzer.Api.Brokers.Loggings;
using EssayAnalyzer.Api.Brokers.Storages;
using EssayAnalyzer.Api.Models.Foundation.Users;
using EssayAnalyzer.Api.Services.Foundation.Users;
using Moq;
using Tynamix.ObjectFiller;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Users;

public partial class UserServiceTests
{
    private readonly Mock<IStorageBroker> storageBrokerMock;
    private readonly Mock<ILoggingBroker> loggingBrokerMock;
    private readonly IUserService userService;
    public UserServiceTests()
    {
        this.storageBrokerMock = new Mock<IStorageBroker>();
        this.loggingBrokerMock = new Mock<ILoggingBroker>();
        this.userService = new UserService(
            storageBroker: storageBrokerMock.Object,
            loggingBroker: loggingBrokerMock.Object);
    }

    private static User CreateRandomUser() =>
        CreateUserFiller().Create();

    private static Filler<User> CreateUserFiller() =>
        new Filler<User>();
}