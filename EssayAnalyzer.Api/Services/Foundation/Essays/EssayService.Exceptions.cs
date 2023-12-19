using EssayAnalyzer.Api.Models.Foundation.Essays;
using EssayAnalyzer.Api.Models.Foundation.Essays.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace EssayAnalyzer.Api.Services.Foundation.Essays;

public partial class EssayService
{
     private delegate ValueTask<Essay> ReturningEssayFunctions();

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
          catch (Exception exception)
          {
               var failedEssayException = new FailedEssayServiceException(exception);
               throw CreateAndLogServiceException(failedEssayException);
          }
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