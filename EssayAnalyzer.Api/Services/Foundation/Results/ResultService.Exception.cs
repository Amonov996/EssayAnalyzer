using EssayAnalyzer.Api.Models.Foundation.Users.Exceptions;
using EssayAnalyzer.Api.Models.Foundation.Results;
using EssayAnalyzer.Api.Models.Foundation.Results.Exceptions;
using Xeptions;
using Microsoft.Data.SqlClient;
using System;

namespace EssayAnalyzer.Api.Services.Foundation.Results;

public partial class ResultService
{
    private delegate ValueTask<Result> ReturnResultFunction();

    private delegate IQueryable<Result> ReturningAllResultsFunction();

    private async ValueTask<Result> TryCatch(ReturnResultFunction returnResultFunction)
    {
        try
        {
            return await returnResultFunction();
        }
        catch (NullResultException nullResultException)
        {
            throw CreateAndLogValidationException(nullResultException);
        }
        catch(InvalidResultException invalidResultException)
        {
            throw CreateAndLogValidationException(invalidResultException);
        }
        catch (SqlException sqlException)
        {
            var failedResultStorageException =
                new FailedResultStorageException(sqlException);

            throw CreateAndLogCriticalDependencyException(failedResultStorageException);
        }
        catch (Exception exception)
        {
            var failedResultServiceException =
                new FailedResultServiceException(exception);

            throw CreateAndLogServiceException(failedResultServiceException);
        }
    }

    private IQueryable<Result> TryCatch(ReturningAllResultsFunction returningAllResultsFunction)
    {
        try
        {
            return returningAllResultsFunction();
        }
        catch (SqlException sqlException)
        {
            var failedResultStorageException =
                new FailedResultStorageException(sqlException);

            throw CreateAndLogCriticalDependencyException(failedResultStorageException);
        }
        catch (Exception exception)
        {
            var failedResultServiceException =
                new FailedResultServiceException(exception);

            throw CreateAndLogServiceException(failedResultServiceException);
        }
    }

    private ResultServiceException CreateAndLogServiceException(Xeption exception)
    {
        var resultServiceException =
            new ResultServiceException(exception);

        this.loggingBroker.LogError(resultServiceException);
        return resultServiceException;
    }

    private ResultDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
    {
        var resultDependencyException = 
            new ResultDependencyException(exception);

        this.loggingBroker.LogCritical(resultDependencyException);
        return resultDependencyException;
    }

    private ResultValidationException CreateAndLogValidationException(Xeption exception)
    {
        var resultValidationException = 
            new ResultValidationException(exception);

        this.loggingBroker.LogError(resultValidationException);
        return resultValidationException;
    }
}