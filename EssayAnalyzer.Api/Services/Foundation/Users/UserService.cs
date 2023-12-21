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

    public ValueTask<User> RetrieveUserByIdAsync(Guid id) =>
        TryCatch(async () =>
        {
            ValidateUserId(id);
            
            User user = await this.storageBroker
                .SelectUserByIdAsync(id);
            
            ValidateUserIsExists(user, id);
            return user;
        });

    public ValueTask<User> ModifyUserAsync(User user) =>
        TryCatch(async () =>
        {
            ValidateUserOnModify(user);

            User updatedUser = await this.storageBroker
                .SelectUserByIdAsync(user.Id);
            
            ValidateUserIsExists(updatedUser, user.Id);
            
            return await this.storageBroker
                .UpdateUserAsync(user);
        });

    public ValueTask<User> RemoveUserByIdAsync(Guid id) =>
        TryCatch(async () =>
        {
            ValidateUserId(id);
            
            User removedUser = await this.storageBroker
                .SelectUserByIdAsync(id);
            
            ValidateUserIsExists(removedUser, id);
            
            return await this.storageBroker
                .DeleteUserAsync(removedUser);
        });
}