using EssayAnalyzer.Api.Models.Foundation.Essays;
using EssayAnalyzer.Api.Models.Foundation.Essays.Exceptions;

namespace EssayAnalyzer.Api.Services.Foundation.Essays;

public partial class EssayService
{
    private static void ValidateEssay(Essay essay)
    {
        ValidateEssayNotNull(essay);
    }
    private static void ValidateEssayNotNull(Essay essay)
    {
        if (essay is null)
        {
            throw new NullEssayException();
        }
    }

 
}