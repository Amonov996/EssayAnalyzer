using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Users.Exceptions
{
    public class UserDependencyException : Xeption
    {
        public UserDependencyException(Xeption innerException)
          : base(message: "User dependency exception occured , contact support",
                innerException)
        { }
    }
}
