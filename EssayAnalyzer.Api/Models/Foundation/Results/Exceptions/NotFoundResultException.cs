using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Results.Exceptions
{
    public class NotFoundResultException : Xeption
    {
        public NotFoundResultException(Guid feedbackId)
          : base(message: $"Couldn't find result with id : {feedbackId}.")
        { }
    }
}
