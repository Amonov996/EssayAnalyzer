using EssayAnalyzer.Api.Models.Foundation.Results;
using FluentAssertions;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Results;

public partial class ResultServiceTests
{
    [Fact]
    public async Task ShouldRetrieveAllResults()
    {
        // given 
        IQueryable<Result> randomResults = CreateRandomResults();
        IQueryable<Result> storagedResults = randomResults;
        IQueryable<Result> expectedResults = storagedResults;

        this.storageBrokerMock.Setup(broker =>
            broker.SelectAllResults()).Returns(storagedResults);

        // when 
        IQueryable<Result> actualResults = this.resultService.RetrieveAllResults();

        // then 
        actualResults.Should().BeEquivalentTo(expectedResults);

        this.storageBrokerMock.Verify(broker =>
            broker.SelectAllResults(), Times.Once);

        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}