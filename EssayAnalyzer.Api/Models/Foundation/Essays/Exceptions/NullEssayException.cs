using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Essays.Exceptions
{
    public class NullEssayException : Xeption
    {
        public NullEssayException()
           : base(message: "Essay is null.")
        { }
    }
}
