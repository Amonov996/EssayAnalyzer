using EssayAnalyzer.Api.Models.Foundation.Essays;
using EssayAnalyzer.Api.Models.Foundation.Results;
using EssayAnalyzer.Api.Models.Orchestration.Analyse;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Orchestration.EssayAnalyseFeedback;

public partial class EssayAnalyseFeedbackOrchestrationServiceTests
{
    [Fact]
    public async Task ShouldCreateEssayFeedbackAnalyseAsync()
    {
        // given
        EssayAnalyse randomEssayAnalyse = CreateRandomEssayAnalyse();
        EssayAnalyse inputEssayAnalyse = randomEssayAnalyse;
        EssayAnalyse persistedEssayAnalyse = inputEssayAnalyse;
        EssayAnalyse expectedEssayAnalyse = persistedEssayAnalyse.DeepClone();

        this.essayAnalyseServiceMock.Setup(service =>
            service.AnalyzeEssayAsync(inputEssayAnalyse.Essay))
            .ReturnsAsync(persistedEssayAnalyse.Result);

        this.essayServiceMock.Setup(service =>
            service.AddEssayAsync(inputEssayAnalyse.Essay))
            .ReturnsAsync(persistedEssayAnalyse.Essay);

        this.resultServiceMock.Setup(service =>
            service.AddResultAsync(inputEssayAnalyse.Result))
            .ReturnsAsync(persistedEssayAnalyse.Result);
        
        // when
        EssayAnalyse actualEssayAnalyse =
            await this._essayAnalyseFeedbackOrchestrationService
                .AnalyseEssayFeedback(inputEssayAnalyse);

        // then
        actualEssayAnalyse.Should().BeEquivalentTo(expectedEssayAnalyse);
        
        this.essayAnalyseServiceMock.Verify(service => 
            service.AnalyzeEssayAsync(inputEssayAnalyse.Essay), Times.Once);
        
        this.essayServiceMock.Verify(service => 
            service.AddEssayAsync(inputEssayAnalyse.Essay), Times.Once);
        
        this.resultServiceMock.Verify(service => 
            service.AddResultAsync(inputEssayAnalyse.Result), Times.Once);
        
        this.essayAnalyseServiceMock.VerifyNoOtherCalls();
        this.essayServiceMock.VerifyNoOtherCalls();
        this.resultServiceMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
    
}