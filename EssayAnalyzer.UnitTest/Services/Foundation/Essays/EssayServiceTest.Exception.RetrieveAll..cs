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
    
}