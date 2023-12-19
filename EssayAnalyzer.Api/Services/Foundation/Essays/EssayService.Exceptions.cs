using EFxceptions.Models.Exceptions;
using EssayAnalyzer.Api.Models.Foundation.Essays;
using EssayAnalyzer.Api.Models.Foundation.Essays.Exceptions;
using EssayAnalyzer.Api.Services.Foundation.Essays.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace EssayAnalyzer.Api.Services.Foundation.Essays;

public partial class EssayService
{
     private delegate ValueTask<Essay> ReturningEssayFunctions();
     private delegate IQueryable<Essay> ReturningEssaysFunctions();

     private async ValueTask<Essay> TryCatch(ReturningEssayFunctions returningEssayFunctions)
     {
          try
          {
               return await returningEssayFunctions();
          }
          catch (NullEssayException nullException)
          {
               throw CreateAdnLogValidationException(nullException);
          }
          catch (InvalidEssayException invalidEssayException)
          {
               throw CreateAdnLogValidationException(invalidEssayException);
          }
          catch (SqlException sqlException)
          {
               var essayStorageException = new FailedEssayStorageException(sqlException);

               throw CreateAndLogCriticalDependencyException(essayStorageException);
          }
          catch (DuplicateKeyException duplicateKeyException)
          {
               var alreadyExistsEssayException = new AlreadyExitsEssayException(duplicateKeyException);

               throw CreateAndLogDependencyValidationException(alreadyExistsEssayException);
          }
          catch (NotFoundEssayException notFoundException)
          {
               throw CreateAdnLogValidationException(notFoundException);

          }
          catch (Exception exception)
          {
               var failedEssayException = new FailedEssayServiceException(exception);
               throw CreateAndLogServiceException(failedEssayException);
          }
     }

     private IQueryable<Essay> TryCatch(ReturningEssaysFunctions returningEssaysFunctions)
     {
          try
          {
               return returningEssaysFunctions();
          }
          catch (SqlException sqlException)
          {
               var failedEssayStorageException = new FailedEssayStorageException(sqlException);

               throw CreateAndLogCriticalDependencyException(failedEssayStorageException);
          }
          catch (Exception exception)
          {
               var failedEssayServiceException = new FailedEssayServiceException(exception);

               throw CreateAndLogServiceException(failedEssayServiceException);
          }
     }

     private EssayDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
     {
          var essayDependencyValidationException = new EssayDependencyValidationException(exception);
          this.loggingBroker.LogError(essayDependencyValidationException);
          
          return essayDependencyValidationException;
     }

     private EssayDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
     {
          var essayDependencyException = new EssayDependencyException(exception);
          this.loggingBroker.LogCritical(essayDependencyException);
          
          return essayDependencyException;
     }

     private EssayValidationException CreateAdnLogValidationException(Xeption exception)
     {
          var essayValidationException = new EssayValidationException(exception);
          this.loggingBroker.LogError(essayValidationException);

          return essayValidationException;
     }

     private Exception CreateAndLogServiceException(Xeption exception)
     {
          var essayServiceException = new EssayServiceException(exception);
          this.loggingBroker.LogError(essayServiceException);

          return essayServiceException;
     }
}