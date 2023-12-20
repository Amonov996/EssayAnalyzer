using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Essays.Exceptions;

public class FailedEssayStorageException : Xeption
{
    public FailedEssayStorageException(Exception innerException) : 
                base(message: "Failed essay storage error occurred, contact support.", innerException) 
    { }
}