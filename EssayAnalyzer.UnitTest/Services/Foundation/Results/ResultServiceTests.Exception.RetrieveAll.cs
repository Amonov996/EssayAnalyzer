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
        actualResultDependencyException.Should().BeEquivalentTo(expectedResultDependencyException);

        this.storageBrokerMock.Verify(broker =>
            broker.SelectAllResults(),Times.Once);

        this.loggingBrokerMock.Verify(broker =>
            broker.LogCritical(It.Is(SameExceptionAs(expectedResultDependencyException))),
            Times.Once);

        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnRetrieveAllIfServiceExceptionOccursAndLogItAsync()
    {
        //given
        string exceptionMessage = GetRandomString();
        var serviceException = new Exception(exceptionMessage);

        var failedResultServiceException =
            new FailedResultServiceException(serviceException);

        var expectedResultServiceException =
            new ResultServiceException(failedResultServiceException);

        this.storageBrokerMock.Setup(broker =>
                broker.SelectAllResults())
            .Throws(serviceException);

        //when
        Action retrieveAllResultAction = () =>
            this.resultService.RetrieveAllResults();

        ResultServiceException actualResultServiceException =
            Assert.Throws<ResultServiceException>(retrieveAllResultAction);

        //then
        actualResultServiceException.Should().BeEquivalentTo(expectedResultServiceException);

        this.storageBrokerMock.Verify(broker =>
            broker.SelectAllResults(), Times.Once);

        this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedResultServiceException))),
            Times.Once);

        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}