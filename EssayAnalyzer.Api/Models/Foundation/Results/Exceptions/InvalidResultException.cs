using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Results.Exceptions
{
    public class InvalidResultException : Xeption
    {
        public InvalidResultException()
          : base(message: "Feedback is invalid.")
        { }
    }
}
