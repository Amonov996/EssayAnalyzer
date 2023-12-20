using EssayAnalyzer.Api.Models.Foundation.Results.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Results;

public partial class ResultServiceTests
{
    [Fact]
    public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlErrorOccursAndLogItAsync()
    {
        // given
        SqlException sqlException = GetSqlException();

        var failedResultStorageException =
            new FailedResultStorageException(sqlException);

        var expectedResultDependencyException =
            new ResultDependencyException(failedResultStorageException);

        this.storageBrokerMock.Setup(broker =>
                broker.SelectAllResults())
            .Throws(sqlException);

        // when
        Action retrieveAllResultsAction = () =>
            this.resultService.RetrieveAllResults();

        ResultDependencyException actualResultDependencyException =
            Assert.Throws<ResultDependencyException>(retrieveAllResultsAction);

        // then
        actualResultDependencyException.Should().BeEquivalentTo(
            expectedResultDependencyException);

        this.storageBrokerMock.Verify(broker =>
                broker.SelectAllResults(),
            Times.Once);

        this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedResultDependencyException))),
            Times.Once);

        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}