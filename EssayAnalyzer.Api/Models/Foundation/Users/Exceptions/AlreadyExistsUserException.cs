using Xeptions;

namespace EssayAnalyzer.Api.Services.Foundation.Users.Exceptions;

public class AlreadyExistsUserException : Xeption
{
    public AlreadyExistsUserException(Exception innerException)
           : base(message: "Already exits exception", innerException)
    { }
}