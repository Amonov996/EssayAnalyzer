using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Results.Exceptions
{
    public class ResultDependencyException : Xeption
    {
        public ResultDependencyException(Xeption innerException)
           : base(message: "Result dependency exception occured , contact support",
                 innerException)
        { }
    }
}
