using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Essays.Exceptions
{
    public class NotFoundEssayException : Xeption
    {
        public NotFoundEssayException(Guid essayId)
           : base(message: $"Couldn't find essay with id : {essayId}.")
        { }
    }
}
