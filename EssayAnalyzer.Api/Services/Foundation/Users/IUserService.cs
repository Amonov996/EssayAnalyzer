using EssayAnalyzer.Api.Models.Foundation.Users;

namespace EssayAnalyzer.Api.Services.Foundation.Users;

public interface IUserService
{
    ValueTask<User> AddUserAsync(User user);
    IQueryable<User> RetrieveAllUsers();
    ValueTask<User> RetrieveUserByIdAsync(Guid id);
    ValueTask<User> ModifyUserAsync(User user);
    ValueTask<User> RemoveUserByIdAsync(Guid id);
}