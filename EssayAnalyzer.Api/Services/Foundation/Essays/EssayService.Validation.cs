using System.Data;
using System.Reflection.Metadata;
using EssayAnalyzer.Api.Models.Foundation.Essays;
using EssayAnalyzer.Api.Models.Foundation.Essays.Exceptions;

namespace EssayAnalyzer.Api.Services.Foundation.Essays;

public partial class EssayService
{
    private static void ValidateEssay(Essay essay)
    {
        ValidateEssayNotNull(essay);
        Validate((Rule: IsInvalid(essay.Id), Parameter: nameof(essay.Id)),
            (Rule: IsInvalid(essay.Content), Parameter: nameof(essay.Content)));

    }

    private static dynamic IsInvalid(Guid id) => new
    {
                Condition = id == Guid.Empty,
                Message = "Id is required"
    };

    private static dynamic IsInvalid(string text) => new
    {
                Condition = string.IsNullOrWhiteSpace(text),
                Message = "Text is required"
    };

    private static void ValidateEssayNotNull(Essay essay)
    {
        if (essay is null)
        {
            throw new NullEssayException();
        }
    }
    
    private static void ValidateEssayId(Guid id) =>
        Validate((Rule:IsInvalid(id), Parameter: nameof(Essay.Id)));

    private void ValidateStorageEssayExists(Essay maybeEssay, Guid id)
    {
        if (maybeEssay is null)
        {
            throw new NotFoundEssayException(id);
        }
    }
    
    private static void Validate(params (dynamic Rule, string Parameter)[] validation)
    {
        var invalidEssayException = new InvalidEssayException();

        foreach ((dynamic rule, string parameter) in validation)
        {
            if (rule.Condition)
            {
                invalidEssayException.UpsertDataList(
                    key: parameter,
                    value: rule.Message);
            }
        }
        invalidEssayException.ThrowIfContainsErrors();
    }
}