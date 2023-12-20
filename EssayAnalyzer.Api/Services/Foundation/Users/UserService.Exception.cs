using EFxceptions.Models.Exceptions;
using EssayAnalyzer.Api.Models.Foundation.Users;
using EssayAnalyzer.Api.Models.Foundation.Users.Exceptions;
using EssayAnalyzer.Api.Services.Foundation.Users.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace EssayAnalyzer.Api.Services.Foundation.Users;

public partial class UserService
{
    private delegate ValueTask<User> ReturnUserFunction();

    private delegate IQueryable<User> ReturnUsersFunction();

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
        catch (NotFoundUserException notFoundUserException)
        {
            throw CreateAndLogValidationException(notFoundUserException);
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
            var invalidUserReferenceException =
                new InvalidUserRefenerenceException(foreignKeyConstraintConflictException);

            throw CreateAndLogDependencyValidationException(invalidUserReferenceException);
        }
        catch (DbUpdateException dbUpdateException)
        {
            var failedUserStorageException =
                new FailedUserStorageException(dbUpdateException);

            throw CreateAndLogDependencyException(failedUserStorageException);
        }
        catch (Exception exception)
        {
            var failedUserServiceException = 
                new FailedUserServiceException(exception);

            throw CreateAndLogServiceException(failedUserServiceException);
        }
    }

    private Exception CreateAndLogServiceException(Xeption exception)
    {
        var userServiceException = 
            new UserServiceException(exception);

        this.loggingBroker.LogError(userServiceException);
        return userServiceException;
    }

    private IQueryable<User> TryCatch(ReturnUsersFunction returnUsersFunction)
    {
        try
        {
            return returnUsersFunction();
        }
        catch (SqlException sqlException)
        {
            var failedUserStorageException =
                new FailedUserStorageException(sqlException);

            throw CreateAndLogCriticalDependencyException(failedUserStorageException);
        }
        catch (Exception exception)
        {
            var failedUserServiceException = 
                new FailedUserServiceException(exception);

            throw CreateAndLogServiceException(failedUserServiceException);
        }
    }

    private UserDependencyException CreateAndLogDependencyException(Xeption exception)
    {
        var userDependencyException = 
            new UserDependencyException(exception);

        this.loggingBroker.LogError(userDependencyException);
        return userDependencyException;
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