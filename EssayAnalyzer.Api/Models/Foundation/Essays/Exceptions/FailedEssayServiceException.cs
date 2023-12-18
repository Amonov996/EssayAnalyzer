using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Essays.Exceptions
{
    public class FailedEssayServiceException : Xeption
    {
        public FailedEssayServiceException(Exception innerException)
           : base(message: "Failed essay service error occured, please contanct support",
                innerException)
        { }
    }
}
