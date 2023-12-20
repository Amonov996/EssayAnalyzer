using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Essays.Exceptions
{
    public class EssayValidationException : Xeption
    {
        public EssayValidationException(Xeption innerException)
            : base(message: "Essay validation error occured, fix the error and try again",
                 innerException)
        { }
    }
}
