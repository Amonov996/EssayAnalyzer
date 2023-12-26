using EssayAnalyzer.Api.Brokers.Loggings;
using EssayAnalyzer.Api.Models.Foundation.Essays;
using EssayAnalyzer.Api.Models.Foundation.Results;
using EssayAnalyzer.Api.Models.Orchestration.Analyse;
using EssayAnalyzer.Api.Services.Foundation.EssayAnalyzes;
using EssayAnalyzer.Api.Services.Foundation.Essays;
using EssayAnalyzer.Api.Services.Foundation.Results;
using EssayAnalyzer.Api.Services.Orchestration.EssayAnalyser;
using Microsoft.AspNetCore.Http;
using Moq;
using Tynamix.ObjectFiller;

namespace EssayAnalyzer.UnitTest.Services.Orchestration.EssayAnalyseFeedback;

public partial class EssayAnalyseFeedbackOrchestrationServiceTests
{
    private readonly Mock<IEssayAnalysisService> essayAnalyseServiceMock;
    private readonly Mock<IEssayService> essayServiceMock;
    private readonly Mock<IResultService> resultServiceMock;
    private readonly Mock<ILoggingBroker> loggingBrokerMock;
    private readonly IEssayAnalyseFeedbackOrchestrationService _essayAnalyseFeedbackOrchestrationService;
    
    public EssayAnalyseFeedbackOrchestrationServiceTests()
    {
        this.essayAnalyseServiceMock = new Mock<IEssayAnalysisService>();
        this.essayServiceMock = new Mock<IEssayService>();
        this.resultServiceMock = new Mock<IResultService>();
        this.loggingBrokerMock = new Mock<ILoggingBroker>();

        this._essayAnalyseFeedbackOrchestrationService = new EssayAnalyserFeedbackOrchestrationService(
            essayAnalysisService: essayAnalyseServiceMock.Object,
            essayService: essayServiceMock.Object,
            resultService: resultServiceMock.Object,
            loggingBroker: loggingBrokerMock.Object);
    }

    private static Essay CreateRandomEssay() =>
        CreateEssayFiller().Create();

    private static Filler<Essay> CreateEssayFiller() =>
        new Filler<Essay>();

    private static Result CreateRandomResult() =>
        CreateResultFiller().Create();

    private static Filler<Result> CreateResultFiller() =>
        new Filler<Result>();

    private static EssayAnalyse CreateRandomEssayAnalyse() =>
        CreateEssayAnalyseFiller().Create();

    private static Filler<EssayAnalyse> CreateEssayAnalyseFiller() =>
        new Filler<EssayAnalyse>();
}