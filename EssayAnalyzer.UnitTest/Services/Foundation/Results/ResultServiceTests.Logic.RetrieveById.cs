using EssayAnalyzer.Api.Models.Foundation.Results;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Results;

public partial class ResultServiceTests
{
    [Fact]
    public async Task ShouldRetrieveResultById()
    {
        // given
        Result randomResult = CreateRandomResult();
        Result persistedResult = randomResult;
        Result expectedResult = persistedResult.DeepClone();

        this.storageBrokerMock.Setup(broker => broker.SelectResultByIdAsync(randomResult.Id))
            .ReturnsAsync(persistedResult);

        // when
        Result actualResult = await this.resultService.RetrieveResultByIdAsync(randomResult.Id);

        // then
        actualResult.Should().BeEquivalentTo(expectedResult);

        this.storageBrokerMock.Verify(broker =>
            broker.SelectResultByIdAsync(randomResult.Id), Times.Once);

        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}