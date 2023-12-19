using EssayAnalyzer.Api.Models.Foundation.Essays.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Essays;

public partial class EssayServiceTest
{
    [Fact]
    public void ShouldThrowsCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
    { 
        //given
        SqlException sqlException = CreateSqlException();
        
        var failedStorageEssayException = 
            new FailedEssayStorageException(sqlException);
        
        var exceptionEssayDependencyException = 
            new EssayDependencyException(failedStorageEssayException);

        this.storageBrokerMock.Setup(broker => 
            broker.SelectAllEssays()).Throws(sqlException);
        
        //when
        Action retrieveAllEssaysAction = () => this.essayService.RetrieveAllEssays();

        EssayDependencyException actualEssayDependencyException =
            Assert.Throws<EssayDependencyException>(retrieveAllEssaysAction);
        
        //then
        actualEssayDependencyException.Should().BeEquivalentTo(exceptionEssayDependencyException);
        
        this.storageBrokerMock.Verify(broker => broker.SelectAllEssays(), Times.Once);
        
        this.loggingBrokerMock.Verify(broker => broker.LogCritical(It.Is(
            SameExceptionAs(exceptionEssayDependencyException))), Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldThrowServiceExceptionOnRetrieveAllEssaysServiceErrorOccursAndLogIt()
    {
        //given
        string exceptionMessage = GetRandomString();
        var serviceException = new Exception();
        
        var failedEssayServiceException = 
            new FailedEssayServiceException(serviceException);
        
        var expectedEssayServiceException = 
            new EssayServiceException(failedEssayServiceException);

        this.storageBrokerMock.Setup(broker => 
            broker.SelectAllEssays()).Throws(serviceException);
        
        //when
        Action retrieveAllEssaysAction = () => this.essayService.RetrieveAllEssays();

        EssayServiceException actualEssayServiceException =
            Assert.Throws<EssayServiceException>(retrieveAllEssaysAction);
        
        //then
        actualEssayServiceException.Should().BeEquivalentTo(expectedEssayServiceException);
        
        this.storageBrokerMock.Verify(broker => broker.SelectAllEssays(), Times.Once);
        
        this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
            SameExceptionAs(expectedEssayServiceException))), Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}