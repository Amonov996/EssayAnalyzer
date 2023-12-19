using EssayAnalyzer.Api.Models.Foundation.Essays;
using EssayAnalyzer.Api.Models.Foundation.Essays.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Essays;

public partial class EssayServiceTest
{
    [Fact]
    public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveEssayBuIdIfSqlErrorOccursAndLogItAsync()
    {
        //given
        Guid someId = Guid.NewGuid();
        SqlException sqlException = CreateSqlException();
        
        var failedEssayStorageException = 
            new FailedEssayStorageException(sqlException);
        
        var exceptedEssayDependencyException = 
            new EssayDependencyException(failedEssayStorageException);

        this.storageBrokerMock.Setup(broker =>
            broker.SelectEssayByIdAsync(It.IsAny<Guid>())).ThrowsAsync(sqlException);

        //when
        ValueTask<Essay> retrieveEssayByIdTask = this.essayService.RetrieveEssayByIdAsync(someId);

        EssayDependencyException actualEssayDependencyException =
            await Assert.ThrowsAsync<EssayDependencyException>(retrieveEssayByIdTask.AsTask);
        
        //then
        actualEssayDependencyException.Should().BeEquivalentTo(exceptedEssayDependencyException);
        
        this.storageBrokerMock.Verify(broker =>
            broker.SelectEssayByIdAsync(It.IsAny<Guid>()), Times.Once);
        
        this.loggingBrokerMock.Verify(broker => broker.LogCritical(It.Is(
            SameExceptionAs(exceptedEssayDependencyException))),
                Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnRetrieveEssayBuIfDatabaseUpdateErrorOccursAndLogItAsync()
    {
        //given
        Guid someId = Guid.NewGuid();
        var serviceException = new Exception();
        var failedEssayException = new FailedEssayServiceException(serviceException);
        var exceptedEssayServiceException = new EssayServiceException(failedEssayException);

        this.storageBrokerMock.Setup(broker => 
            broker.SelectEssayByIdAsync(It.IsAny<Guid>())).ThrowsAsync(serviceException);
        
        //when
        ValueTask<Essay> retrieveEssayByIdTask = this.essayService.RetrieveEssayByIdAsync(someId);

        EssayServiceException actualEssayServiceException =
            await Assert.ThrowsAsync<EssayServiceException>(retrieveEssayByIdTask.AsTask);
        
        //then
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectEssayByIdAsync(It.IsAny<Guid>()), Times.Once);
        
        this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
            SameExceptionAs(exceptedEssayServiceException))), Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}