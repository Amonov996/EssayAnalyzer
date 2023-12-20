using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Results.Exceptions
{
    public class ResultValidationException : Xeption
    {
        public ResultValidationException(Xeption innerException)
           : base(message: "Result is invalid, fix the the error and try again.",
                 innerException)
        { }
    }
}
