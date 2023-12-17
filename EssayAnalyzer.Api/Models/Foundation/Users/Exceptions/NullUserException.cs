using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Users.Exceptions
{
    public class NullUserException : Xeption
    {
        public NullUserException()
            : base(message: "User is null.")
        { }
    }
}
