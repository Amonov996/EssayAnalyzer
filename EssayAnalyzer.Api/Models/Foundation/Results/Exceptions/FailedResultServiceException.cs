using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Results.Exceptions
{
    public class FailedResultServiceException : Xeption
    {
        public FailedResultServiceException(Exception innerException)
           : base(message: "Failed result service error occured, please contact support",
                 innerException)
        { }
    }
}
