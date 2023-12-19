using EFxceptions.Models.Exceptions;
using EssayAnalyzer.Api.Models.Foundation.Essays;
using EssayAnalyzer.Api.Models.Foundation.Essays.Exceptions;
using EssayAnalyzer.Api.Services.Foundation.Essays.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Essays;

public partial class EssayServiceTest
{
    [Fact]
    public async Task ShouldThrowCriticalDependencyExceptionIfSqlErrorOccursAndLogItAsync()
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
        actualEssayDependencyException.Should().BeEquivalentTo(expectedEssayStorageException);

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

    [Fact] public async Task ShouldThrowDependencyValidationExceptionOnAddIfDuplicateKeyErrorOccursAndLogItAsync()
    {
        //given
        Essay randomEssay = CreateRandomEssay();
        string randomMessage = GetRandomString();

        var duplicateKeyException = new DuplicateKeyException(randomMessage);
        var alreadyExistsEssayException = new AlreadyExitsEssayException(duplicateKeyException);
        
        var expectedEssayDependencyValidationException =
            new EssayDependencyValidationException(alreadyExistsEssayException);

        this.storageBrokerMock.Setup(broker =>
            broker.InsertEssayAsync(randomEssay)).ThrowsAsync(duplicateKeyException);
        
        //when
        ValueTask<Essay> addEssayTask = this.essayService.AddEssayAsync(randomEssay);

        EssayDependencyValidationException actualEssayDependencyValidationException =
            await Assert.ThrowsAsync<EssayDependencyValidationException>(addEssayTask.AsTask);
        
        //then
        actualEssayDependencyValidationException.Should()
            .BeEquivalentTo(expectedEssayDependencyValidationException);
        
        this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
            SameExceptionAs(expectedEssayDependencyValidationException))), Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
                broker.InsertEssayAsync(It.IsAny<Essay>()), Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
    {
        //given
        Essay randomEssay = CreateRandomEssay();
        var serviceException = new Exception();

        var failedEssayServiceException = new FailedEssayServiceException(serviceException);
        var expectedEssayServiceException = new EssayServiceException(failedEssayServiceException);

        this.storageBrokerMock.Setup(broker => 
            broker.InsertEssayAsync(randomEssay)).ThrowsAsync(serviceException);
        
        //when
        ValueTask<Essay> addEssayTask = this.essayService.AddEssayAsync(randomEssay);

        EssayServiceException actualEssayServiceException =
            await Assert.ThrowsAsync<EssayServiceException>(addEssayTask.AsTask);
        
        //then
        actualEssayServiceException.Should().BeEquivalentTo(expectedEssayServiceException);

        this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
            SameExceptionAs(expectedEssayServiceException))), Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
            broker.InsertEssayAsync(randomEssay), Times.Once);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}