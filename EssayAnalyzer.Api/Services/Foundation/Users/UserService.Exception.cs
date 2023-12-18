using EssayAnalyzer.Api.Models.Foundation.Users;
using EssayAnalyzer.Api.Models.Foundation.Users.Exceptions;
using Xeptions;

namespace EssayAnalyzer.Api.Services.Foundation.Users;

public partial class UserService
{
    private delegate ValueTask<User> ReturnUserFunction();

    private async ValueTask<User> TryCatch(ReturnUserFunction returnUserFunction)
    {
        try
        {
            return await returnUserFunction();
        }
        catch (NullUserException nullUserException)
        {
            throw CreateAndLogValidationException(nullUserException);
        }
        catch (InvalidUserException invalidUserException)
        {
            throw CreateAndLogValidationException(invalidUserException);
        }
    }

    private UserValidationException CreateAndLogValidationException(Xeption exception)
    {
        UserValidationException userValidationException = 
            new UserValidationException(exception);

        this.loggingBroker.LogError(userValidationException);
        return userValidationException;
    }
}