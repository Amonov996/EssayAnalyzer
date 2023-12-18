using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Results.Exceptions
{
    public class ResultDependencyValidationException : Xeption
    {
        public ResultDependencyValidationException(Xeption innerException)
         : base(message: "Result dependency validation exception occured, fix the errors and try again.",
               innerException)
        { }
    }
}
