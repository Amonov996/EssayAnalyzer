using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Users.Exceptions
{
    public class UserServiceException : Xeption
    {
        public UserServiceException(Xeption innerException)
          : base(message: "User service error occured , contact support.",
               innerException)
        { }
    }
}
