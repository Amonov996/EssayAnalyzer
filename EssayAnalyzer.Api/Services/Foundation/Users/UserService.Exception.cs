using EFxceptions.Models.Exceptions;
using EssayAnalyzer.Api.Models.Foundation.Users;
using EssayAnalyzer.Api.Models.Foundation.Users.Exceptions;
using EssayAnalyzer.Api.Services.Foundation.Users.Exceptions;
using Microsoft.Data.SqlClient;
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
        catch (SqlException sqlException)
        {
            var failedUserStorageException =
                new FailedUserStorageException(sqlException);

            throw CreateAndLogCriticalDependencyException(failedUserStorageException);
        }
        catch (DuplicateKeyException duplicateKeyException)
        {
            var alreadyExistsUserException =
                new AlreadyExistsUserException(duplicateKeyException);

            throw CreateAndLogDependencyValidationException(alreadyExistsUserException);
        }
        catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
        {
            var invalidUserRefenerenceException =
                new InvalidUserRefenerenceException(foreignKeyConstraintConflictException);

            throw CreateAndLogDependencyValidationException(invalidUserRefenerenceException);
        }
    }

    private UserDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
    {
        var userDependencyValidationException =
            new UserDependencyValidationException(exception);

        this.loggingBroker.LogError(userDependencyValidationException);
        return userDependencyValidationException;
    }

    private UserDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
    {
        var userDependencyException = 
            new UserDependencyException(exception);

        this.loggingBroker.LogCritical(userDependencyException);
        return userDependencyException;
    }

    private UserValidationException CreateAndLogValidationException(Xeption exception)
    {
        UserValidationException userValidationException = 
            new UserValidationException(exception);

        this.loggingBroker.LogError(userValidationException);
        return userValidationException;
    }
}