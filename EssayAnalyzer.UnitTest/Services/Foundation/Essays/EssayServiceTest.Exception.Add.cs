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
    public async Task ShouldThrowsCriticalExceptionOnAddIfErrorOccursAndLogItAsync()
    {
        //given
        Essay randomEssay = CreateRandomEssay();
        SqlException sqlException = CreateSqlException();

        var failedEssayStorageException = new FailedEssayStorageException(sqlException);
        var expectedEssayStorageException = new EssayDependencyException(failedEssayStorageException);

        this.storageBrokerMock.Setup(broker =>
            broker.InsertEssayAsync(randomEssay)).ThrowsAsync(sqlException);
        
        //when
        ValueTask<Essay> addEssayTask = this.essayService.AddEssayAsync(randomEssay);

        EssayDependencyException actualEssayDependencyException =
            await Assert.ThrowsAnyAsync<EssayDependencyException>(addEssayTask.AsTask);
        
        //then
        actualEssayDependencyException.Should().BeEquivalentTo(
            expectedEssayStorageException);

        this.storageBrokerMock.Verify(broker =>
            broker.InsertEssayAsync(randomEssay), 
                Times.Once);
        
        this.loggingBrokerMock.Verify(broker =>
            broker.LogCritical(It.Is(SameExceptionAs(
                expectedEssayStorageException))),
                    Times.Once);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}