using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Users.Exceptions
{
    public class UserValidationException : Xeption
    {
        public UserValidationException(Xeption innerException)
          : base(message: "User validation error occured, fix the errors and try again.",
               innerException)
        { }
    }
}
