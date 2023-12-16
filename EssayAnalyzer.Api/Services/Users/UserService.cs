using EssayAnalyzer.Api.Brokers.Loggings;
using EssayAnalyzer.Api.Brokers.Storages;
using EssayAnalyzer.Api.Models.Foundation.Users;

namespace EssayAnalyzer.Api.Services.Users;

public class UserService : IUserService
{
    private readonly IStorageBroker storageBroker;
    private readonly ILoggingBroker loggingBroker;

    public UserService(IStorageBroker storageBroker,
        ILoggingBroker loggingBroker)
    {
        this.storageBroker = storageBroker;
        this.loggingBroker = loggingBroker;
    }
    
    public async ValueTask<User> InsertUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<User> SelectUserByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<User> SelectAllUsers()
    {
        throw new NotImplementedException();
    }

    public async ValueTask<User> UpdateUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<User> RemoveUserById(Guid id)
    {
        throw new NotImplementedException();
    }
}