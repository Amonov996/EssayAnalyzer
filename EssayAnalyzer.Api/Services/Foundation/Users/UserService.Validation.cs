using EssayAnalyzer.Api.Models.Foundation.Users;
using EssayAnalyzer.Api.Models.Foundation.Users.Exceptions;

namespace EssayAnalyzer.Api.Services.Foundation.Users;

public partial class UserService
{
    private static void ValidateUserOnAdd(User user)
    {
        ValidateUserIsNotNull(user);
    }

    private static void ValidateUserIsNotNull(User user)
    {
        if (user is null)
        {
            throw new NullUserException();
        }
    }
}