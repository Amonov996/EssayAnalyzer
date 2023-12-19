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
            key: nameof(Essay.Id));

        var expectedEssayValidationException =
            new EssayValidationException(invalidEssayException);
        
        //when
        ValueTask<Essay> retrieveEssayByIdTask =
            this.essayService.RemoveEssayByIdAsync(invalidEssayId);

        EssayValidationException actualEssayValidationException =
            await Assert.ThrowsAsync<EssayValidationException>(retrieveEssayByIdTask.AsTask);
        
        //then
        actualEssayValidationException.Should().BeEquivalentTo(expectedEssayValidationException);

        this.loggingBrokerMock.Verify(broker => broker.LogCritical(It.Is(
            SameExceptionAs(expectedEssayValidationException))), Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
                broker.SelectEssayByIdAsync(It.IsAny<Guid>()), Times.Never);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}