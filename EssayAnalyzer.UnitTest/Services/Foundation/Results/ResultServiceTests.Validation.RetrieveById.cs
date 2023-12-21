using EssayAnalyzer.Api.Models.Foundation.Results;
using EssayAnalyzer.Api.Models.Foundation.Results.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Results;

public partial class ResultServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
    {
        // given
        Guid invalidResultId = Guid.Empty;
        var invalidResultException = new InvalidResultException();

        invalidResultException.AddData(
            key: nameof(Result.Id),
            values: "Id is required");

        var expectedResultValidationException =
            new ResultValidationException(invalidResultException);

        // when
        ValueTask<Result> retrieveResultByIdTask =
            this.resultService.RetrieveResultByIdAsync(invalidResultId);

        ResultValidationException actualResultValidationException =
            await Assert.ThrowsAsync<ResultValidationException>(retrieveResultByIdTask.AsTask);

        // then
        actualResultValidationException.Should().BeEquivalentTo(expectedResultValidationException);

        this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedResultValidationException))),
            Times.Once);

        this.storageBrokerMock.Verify(broker =>
            broker.SelectResultByIdAsync(It.IsAny<Guid>()),Times.Never);

        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfResultIsNotFoundAndLogItAsync()
    {
        // given
        Guid someResultId = Guid.NewGuid();
        Result noResult = null;
        var notFoundResultException = new NotFoundResultException(someResultId);

        var expectedResultValidationException =
            new ResultValidationException(notFoundResultException);

        this.storageBrokerMock.Setup(broker =>
            broker.SelectResultByIdAsync(It.IsAny<Guid>()))!.ReturnsAsync(noResult);

        // when
        ValueTask<Result> retrieveResultByIdTask = this.resultService.RetrieveResultByIdAsync(someResultId);

        ResultValidationException actualResultValidationException =
            await Assert.ThrowsAsync<ResultValidationException>(retrieveResultByIdTask.AsTask);

        // then
        actualResultValidationException.Should().BeEquivalentTo(expectedResultValidationException);

        this.storageBrokerMock.Verify(broker =>
            broker.SelectResultByIdAsync(It.IsAny<Guid>()), Times.Once);

        this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedResultValidationException))),
            Times.Once);

        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}