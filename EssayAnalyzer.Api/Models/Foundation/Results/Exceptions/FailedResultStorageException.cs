using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Results.Exceptions
{
    public class FailedResultStorageException : Xeption
    {
        public FailedResultStorageException(Exception innerException)
          : base(message: "Failed result storage exception occured, contact support",
                innerException)
        { }
    }
}
