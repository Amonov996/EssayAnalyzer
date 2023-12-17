using System.Linq.Expressions;
using System.Runtime.Serialization;
using EssayAnalyzer.Api.Brokers.Loggings;
using EssayAnalyzer.Api.Brokers.Storages;
using EssayAnalyzer.Api.Models.Foundation.Essays;
using EssayAnalyzer.Api.Models.Foundation.Results;
using EssayAnalyzer.Api.Services.Foundation.Essays;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Essays;

public class EssayServiceTest
{
    private readonly Mock<IStorageBroker> storageBrokerMock;
    private readonly Mock<ILoggingBroker> loggingBrokerMock;
    private readonly IEssayService essayService;

    public EssayServiceTest()
    {
        this.storageBrokerMock = new Mock<IStorageBroker>(); 
        this.loggingBrokerMock = new Mock<ILoggingBroker>();

        this.essayService = new EssayService(
            storageBroker: this.storageBrokerMock.Object,
            loggingBroker: this.loggingBrokerMock.Object);
    }

    private static Essay CreateRandomEssay() => 
                CreateEssayFiller().Create();

    private static string GetRandomString() => 
                new MnemonicString(wordCount: GetRandomNumber()).GetValue();

    private static int GetRandomNumber() => 
                new IntRange(min: 9, max: 99).GetValue();

    private static SqlException CreateSqlException() => 
                (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

    private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) => 
                actualException => actualException.SameExceptionAs(expectedException);
    
    
    private static Filler<Essay> CreateEssayFiller()
    { 
        var filler = new Filler<Essay>();
        filler.Setup().OnType<Result>().IgnoreIt();

        return filler;
    }
}