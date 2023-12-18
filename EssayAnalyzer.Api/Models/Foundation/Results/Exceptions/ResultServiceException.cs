using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Results.Exceptions
{
    public class ResultServiceException : Xeption
    {
        public ResultServiceException(Xeption innerException)
          : base(message: "Feedback service error occured , contact support.",
               innerException)
        { }
    }
}
