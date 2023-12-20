using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Essays.Exceptions
{
    public class EssayDependencyException : Xeption
    {
        public EssayDependencyException(Exception innerException)
            : base(message: "Essay dependency error occured, contact support.",
                  innerException)
        { }
    }
}
