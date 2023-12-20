using EssayAnalyzer.Api.Models.Foundation.Results;
using EssayAnalyzer.Api.Models.Foundation.Results.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Results;

public partial class ResultServiceTests
{
    [Fact]
    public async Task ShouldThrowCriticalDependencyExceptionIfSqlErrorOccursAndLogItAsync()
    {
        // given
        Result randomResult = CreateRandomResult();
        SqlException sqlException = GetSqlException();

        var failedResultStorageException = 
            new FailedResultStorageException(sqlException);

        var expectedResultDependencyException =
            new ResultDependencyException(failedResultStorageException);

        this.storageBrokerMock.Setup(broker =>
            broker.InsertResultAsync(randomResult)).ThrowsAsync(sqlException);

        // when
        ValueTask<Result> addResultTask = this.resultService.AddResultAsync(randomResult);

        var actualResultDependencyException =
            await Assert.ThrowsAsync<ResultDependencyException>(addResultTask.AsTask);

        // then
        actualResultDependencyException.Should().BeEquivalentTo(expectedResultDependencyException);

        this.loggingBrokerMock.Verify(broker =>
            broker.LogCritical(It.Is(SameExceptionAs(
                expectedResultDependencyException))), Times.Once);

        this.storageBrokerMock.Verify(broker =>
            broker.InsertResultAsync(randomResult), Times.Once);

        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
    {
        // given
        Result randomResult = CreateRandomResult();
        Exception serviceException = new Exception();

        var failedResultServiceException =
            new FailedResultServiceException(serviceException);

        var expectedResultServiceException =
            new ResultServiceException(failedResultServiceException);

        this.storageBrokerMock.Setup(broker =>
            broker.InsertResultAsync(randomResult)).ThrowsAsync(serviceException);

        // when
        ValueTask<Result> addResultTask = this.resultService.AddResultAsync(randomResult);

        var actuaResultServiceException =
            await Assert.ThrowsAsync<ResultServiceException>(addResultTask.AsTask);

        // then
        actuaResultServiceException.Should().BeEquivalentTo(expectedResultServiceException);

        this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedResultServiceException))),
            Times.Once);

        this.storageBrokerMock.Verify(broker =>
            broker.InsertResultAsync(randomResult), Times.Once);

        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}