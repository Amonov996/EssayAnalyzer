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
    public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
    {
        // given
        Guid someResultId = Guid.NewGuid();
        SqlException sqlException = GetSqlException();

        var failedResultStorageException =
            new FailedResultStorageException(sqlException);

        var expectedResultDependencyException =
            new ResultDependencyException(failedResultStorageException);

        this.storageBrokerMock.Setup(broker =>
            broker.SelectResultByIdAsync(It.IsAny<Guid>())).ThrowsAsync(sqlException);

        // when
        ValueTask<Result> retrieveResultByIdTask = this.resultService.RetrieveResultByIdAsync(someResultId);

        ResultDependencyException actualResultDependencyException =
            await Assert.ThrowsAsync<ResultDependencyException>(retrieveResultByIdTask.AsTask);

        // then
        actualResultDependencyException.Should().BeEquivalentTo(expectedResultDependencyException);

        this.storageBrokerMock.Verify(broker =>
            broker.SelectResultByIdAsync(It.IsAny<Guid>()), Times.Once);

        this.loggingBrokerMock.Verify(broker =>
            broker.LogCritical(It.Is(SameExceptionAs(expectedResultDependencyException))),
            Times.Once);

        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}