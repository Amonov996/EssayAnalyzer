using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Results.Exceptions
{
    public class LockedResultException : Xeption
    {
        public LockedResultException(Exception innerException)
         : base(message: "Result is locked , please try again.",
               innerException)
        { }
    }
}
