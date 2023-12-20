using EssayAnalyzer.Api.Brokers.Loggings;
using EssayAnalyzer.Api.Brokers.Storages;
using EssayAnalyzer.Api.Models.Foundation.Users;

namespace EssayAnalyzer.Api.Services.Foundation.Users;

public partial class UserService : IUserService
{
    private readonly IStorageBroker storageBroker;
    private readonly ILoggingBroker loggingBroker;

    public UserService(IStorageBroker storageBroker,
        ILoggingBroker loggingBroker)
    {
        this.storageBroker = storageBroker;
        this.loggingBroker = loggingBroker;
    }

    public  ValueTask<User> AddUserAsync(User user) =>
        TryCatch(async () =>
    {
        ValidateUserOnAdd(user);
        return await this.storageBroker.InsertUserAsync(user);
    });

    public IQueryable<User> RetrieveAllUsers() =>
        TryCatch(() => this.storageBroker.SelectAllUsers());
    
    public async ValueTask<User> RetrieveUserByIdAsync(Guid id)
    {
        return await this.storageBroker.SelectUserByIdAsync(id);
    }

    public async ValueTask<User> ModifyUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<User> RemoveUserByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}