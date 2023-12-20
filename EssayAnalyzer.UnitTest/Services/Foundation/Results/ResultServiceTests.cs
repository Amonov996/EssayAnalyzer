using EssayAnalyzer.Api.Brokers.Loggings;
using EssayAnalyzer.Api.Brokers.Storages;
using EssayAnalyzer.Api.Models.Foundation.Results;
using EssayAnalyzer.Api.Services.Foundation.Results;
using Microsoft.Data.SqlClient;
using Moq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Tynamix.ObjectFiller;
using Xeptions;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Results;

public partial class ResultServiceTests
{
    private readonly Mock<IStorageBroker> storageBrokerMock;
    private readonly Mock<ILoggingBroker> loggingBrokerMock;
    private readonly IResultService resultService;

    public ResultServiceTests()
    {
        this.storageBrokerMock = new Mock<IStorageBroker>();
        this.loggingBrokerMock = new Mock<ILoggingBroker>();

        this.resultService = new ResultService(
            storageBroker: this.storageBrokerMock.Object,
            loggingBroker: this.loggingBrokerMock.Object);
    }

    private static IQueryable<Result> CreateRandomResults()
    {
        return CreateResultFiller()
            .Create(count: GetRandomNumber())
            .AsQueryable();
    }

    private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
        actualException => actualException.SameExceptionAs(expectedException);

    private static SqlException GetSqlException() =>
        (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

    private static Result CreateRandomResult() =>
        CreateResultFiller().Create();

    private static Filler<Result> CreateResultFiller() =>
        new Filler<Result>();

    private static int GetRandomNumber() =>
        new IntRange(min: 0, max: 99).GetValue();

    private static string GetRandomMessage() =>
        new MnemonicString(wordCount: GetRandomNumber()).GetValue();

    private static string GetRandomString() =>
        new MnemonicString(wordCount: GetRandomNumber()).GetValue();
}