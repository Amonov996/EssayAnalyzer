using Xeptions;

namespace EssayAnalyzer.Api.Services.Foundation.Results.Exceptions;

public class AlreadyExitsResultException : Xeption
{
    public AlreadyExitsResultException(Exception innerException)
      : base(message: "Result already exists.",
        innerException)
    { }
}