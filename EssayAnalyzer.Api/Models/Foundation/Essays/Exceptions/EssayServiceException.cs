using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Essays.Exceptions
{
    public class EssayServiceException : Xeption
    {
        public EssayServiceException(Exception innerException)
          : base(message: "Essay service error occured , contact support.",
                innerException)
        { }
    }
}
