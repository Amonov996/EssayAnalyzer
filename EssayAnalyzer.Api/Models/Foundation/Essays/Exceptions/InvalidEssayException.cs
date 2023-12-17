using Xeptions;

namespace EssayAnalyzer.Api.Models.Foundation.Essays.Exceptions
{
    public class InvalidEssayException : Xeption
    {
        public InvalidEssayException()
           : base(message: "Essay is invalid.")
        { }
    }
}
