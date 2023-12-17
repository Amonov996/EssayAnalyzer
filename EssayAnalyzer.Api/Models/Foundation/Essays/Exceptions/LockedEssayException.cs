using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Essays.Exceptions
{
    public class LockedEssayException : Xeption
    {
        public LockedEssayException(Exception innerException)
            : base(message: "Locked essay record occured, contact support",
                  innerException)
        { }
    }
}
