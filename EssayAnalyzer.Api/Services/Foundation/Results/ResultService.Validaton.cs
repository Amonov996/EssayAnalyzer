using EssayAnalyzer.Api.Models.Foundation.Results;
using EssayAnalyzer.Api.Models.Foundation.Results.Exceptions;

namespace EssayAnalyzer.Api.Services.Foundation.Results;

public partial class ResultService
{
    private static void ValidateResultOnAdd(Result result)
    {
        ValidateResultIsNotNull(result);

        Validate(
                (Rule: IsInvalid(result.Id), Parametr: nameof(Result.Id)),
                (Rule: IsInvalid(result.EssayId), Parametr: nameof(Result.EssayId)),
                (Rule: IsInvalid(result.Feedback), Parametr: nameof(Result.Feedback)));
    }

    private static void ValidateResultIsNotNull(Result result)
    {
        if (result is null)
        {
            throw new NullResultException();
        }
    }

    private static void ValidateResultId(Guid id) =>
        Validate((Rule: IsInvalid(id), Parametr: nameof(Result.Id)));

    private static void ValidateResultIsExists(Result result, Guid id)
    {
        if (result is null)
        {
            throw new NotFoundResultException(id);
        }
    }

    private static dynamic IsInvalid(Guid id) => new
    {
        Condition = id == Guid.Empty,
        Message = "Id is required"
    };

    private static dynamic IsInvalid(string text) => new
    {
        Condition = String.IsNullOrWhiteSpace(text),
        Message = "Text is required"
    };

    private static void Validate(params (dynamic Rule, string Parametr)[] validations)
    {
        var invalidResultException = new InvalidResultException();

        foreach ((dynamic rule, string parametr) in validations)
        {
            if (rule.Condition)
            {
                invalidResultException.UpsertDataList(
                    key: parametr,
                    value: rule.Message);
            }
        }

        invalidResultException.ThrowIfContainsErrors();
    }
}