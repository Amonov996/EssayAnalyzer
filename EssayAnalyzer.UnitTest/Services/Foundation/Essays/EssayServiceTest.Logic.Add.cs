using EssayAnalyzer.Api.Models.Foundation.Essays;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Essays;

public partial class EssayServiceTest
{
    [Fact]
    public async Task ShouldAddEssayAsync()
    {

        //given
        Essay randomEssay = CreateRandomEssay();
        Essay inputEssay = randomEssay;
        Essay persistedEssay = inputEssay;
        Essay expectedEssay = persistedEssay.DeepClone();

        this.storageBrokerMock.Setup(broker =>
                    broker.InsertEssayAsync(inputEssay)).ReturnsAsync(persistedEssay);

        // when
        Essay actualEssay = await this.essayService.AddEssayAsync(inputEssay);

        //then
        actualEssay.Should().BeEquivalentTo(expectedEssay);

        this.storageBrokerMock.Verify(broker =>
                    broker.InsertEssayAsync(actualEssay), Times.Once);

        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();

    }
}