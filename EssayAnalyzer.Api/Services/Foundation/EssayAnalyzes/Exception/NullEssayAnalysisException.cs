using Xeptions;

namespace EssayAnalyzer.Api.Services.Foundation.EssayAnalyzes.Exception;

public class NullEssayAnalysisException : Xeption
{
    public NullEssayAnalysisException()
        : base(message: "Chat completion is null.")
    { }

    public NullEssayAnalysisException(string message, Xeption innerException)
        : base(message, innerException)
    { }
}