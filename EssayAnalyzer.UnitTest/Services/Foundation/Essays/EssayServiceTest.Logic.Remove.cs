using EssayAnalyzer.Api.Models.Foundation.Essays;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Essays;

public partial class EssayServiceTest
{
    [Fact]
    public async Task ShouldRemoveEssayAsync()
    {
        // given
        Guid randomId = Guid.NewGuid();
        Guid inputId = randomId;
        Essay randomEssay = CreateRandomEssay();
        Essay persistedEssay = randomEssay;
        Essay expectedEssayInput = persistedEssay;
        Essay deletedEssay = expectedEssayInput;
        Essay expectedEssay = deletedEssay.DeepClone();

        this.storageBrokerMock.Setup(broker =>
            broker.SelectEssayByIdAsync(inputId))
            .ReturnsAsync(persistedEssay);

        this.storageBrokerMock.Setup(broker =>
            broker.DeleteEssayAsync(expectedEssayInput))
            .ReturnsAsync(deletedEssay);

        // when
        Essay actualEssay = await this.essayService
            .RemoveEssayByIdAsync(inputId);

        // then
        actualEssay.Should().BeEquivalentTo(expectedEssay);

        this.storageBrokerMock.Verify(broker =>
            broker.SelectEssayByIdAsync(inputId), Times.Once);

        this.storageBrokerMock.Verify(broker =>
            broker.DeleteEssayAsync(persistedEssay), Times.Once);

        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}