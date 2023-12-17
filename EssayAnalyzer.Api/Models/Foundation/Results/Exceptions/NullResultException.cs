using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Results.Exceptions
{
    public class NullResultException : Xeption
    {
        public NullResultException()
         : base(message: "Result is null.")
        { }
    }
}
