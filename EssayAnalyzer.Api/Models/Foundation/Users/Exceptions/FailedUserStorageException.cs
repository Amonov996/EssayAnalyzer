using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Users.Exceptions;

public class FailedUserStorageException : Xeption
{
    public FailedUserStorageException(Exception innerException)
        : base("User failed error occured, contact support.", innerException)
    { }
}