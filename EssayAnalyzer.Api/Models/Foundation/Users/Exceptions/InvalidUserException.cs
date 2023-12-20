using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Users.Exceptions
{
    public class InvalidUserException : Xeption
    {
        public InvalidUserException()
        : base(message: "User is invalid.")
        { }
    }
}
