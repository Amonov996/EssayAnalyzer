using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Users.Exceptions
{
    public class NotFoundUserException : Xeption
    {
        public NotFoundUserException(Guid userId)
          : base(message: $"Couldn't found user with id: {userId}")
        { }
    }
}
