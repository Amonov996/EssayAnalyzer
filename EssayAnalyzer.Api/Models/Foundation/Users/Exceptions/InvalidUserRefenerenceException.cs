using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Users.Exceptions;

public class InvalidUserRefenerenceException : Xeption
{
    public InvalidUserRefenerenceException(Exception innerException)
        : base("Invalid reference error occured.", innerException)
    { }
}