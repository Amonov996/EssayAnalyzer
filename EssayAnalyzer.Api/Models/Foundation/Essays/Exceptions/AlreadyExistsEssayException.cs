using Xeptions;

namespace EssayAnalyzer.Api.Services.Foundation.Essays.Exceptions;

public class AlreadyExitsEssayException : Xeption
{
    public AlreadyExitsEssayException(Exception innerException)
         : base(message: "Essay already exists.",
           innerException)
    { }
}