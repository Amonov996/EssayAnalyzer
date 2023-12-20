using EssayAnalyzer.Api.Models.Foundation.Essays;
using EssayAnalyzer.Api.Models.Foundation.Essays.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Essays;

public partial class EssayServiceTest
{
    [Fact]
    public async Task ShouldThrowsValidationExceptionOnRetrieveEssayByIdAndLogItAsync()
    {
        //given
        Guid invalidEssayId = Guid.Empty;
        var invalidEssayException = new InvalidEssayException();
        
        invalidEssayException.AddData(
            key: nameof(Essay.Id),
            values: "Id is required");

        var expectedEssayValidationException =
            new EssayValidationException(invalidEssayException);
        
        //when
        ValueTask<Essay> retrieveEssayByIdTask =
            this.essayService.RetrieveEssayByIdAsync(invalidEssayId);

        EssayValidationException actualEssayValidationException =
            await Assert.ThrowsAsync<EssayValidationException>(retrieveEssayByIdTask.AsTask);
        
        //then
        actualEssayValidationException.Should().BeEquivalentTo(expectedEssayValidationException);

        this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
            SameExceptionAs(expectedEssayValidationException))), Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
                broker.SelectEssayByIdAsync(It.IsAny<Guid>()), Times.Never);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionOnRetrieveEssayByIdIfEssayNotFoundAndLogItAsync()
    {
        //given
        Guid someEssayId = Guid.NewGuid();
        Essay? noEssay = null;
        
        var notFoundEssayException = 
            new NotFoundEssayException(someEssayId);
        
        var exceptedEssayValidationException = 
            new EssayValidationException(notFoundEssayException);

        this.storageBrokerMock.Setup(broker => 
            broker.SelectEssayByIdAsync(It.IsAny<Guid>())).ReturnsAsync(noEssay);
        
        //when
        ValueTask<Essay> retrieveEssayByIdTask = 
            this.essayService.RetrieveEssayByIdAsync(someEssayId);

        EssayValidationException actualEssayValidationException =
            await Assert.ThrowsAsync<EssayValidationException>(retrieveEssayByIdTask.AsTask);
        
        //then
        actualEssayValidationException.Should().BeEquivalentTo(exceptedEssayValidationException);

        this.storageBrokerMock.Verify(broker =>
            broker.SelectEssayByIdAsync(It.IsAny<Guid>()), Times.Once);
        
        this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
            SameExceptionAs(exceptedEssayValidationException))), Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}