using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Users.Exceptions
{
    public class UserDependencyValidationException : Xeption
    {
        public UserDependencyValidationException(Xeption innerException)
          : base(message: "User dependency validation exception occured, fix the errors and try again.",
                innerException)
        { }
    }
}
