using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Essays.Exceptions
{
    public class EssayDependencyValidationException : Xeption
    {
        public EssayDependencyValidationException(Xeption innerException)
            : base(message: "Essay Dependency validation error occured , fix the errors and try again",
                  innerException)
        { }
    }
}
