using EssayAnalyzer.Api.Models.Foundation.Users;

namespace EssayAnalyzer.Api.Services.Users;

public interface IUserService
{
    ValueTask<User> InsertUserAsync(User user);
    ValueTask<User> SelectUserByIdAsync(Guid id);
    IQueryable<User> SelectAllUsers();
    ValueTask<User> UpdateUserAsync(User user);
    ValueTask<User> RemoveUserById(Guid id);
}