using EssayAnalyzer.Api.Models.Foundation.Essays;
using EssayAnalyzer.Api.Models.Foundation.Essays.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Essays;

public partial class EssayServiceTest
{
    [Fact]
    public async Task ShouldValidateExceptionOnAddIfInputIsNullAndLogItAsync()
    {
        //given
        Essay nullEssay = null;
        var nullEssayException = new NullEssayException();
        
        var expectedEssayValidationException = 
                    new EssayValidationException(nullEssayException);

        //when
        ValueTask<Essay> addEssayTask = this.essayService.AddEssayAsync(nullEssay);

        EssayValidationException actualEssayValidationException =
                    await Assert.ThrowsAsync<EssayValidationException>(addEssayTask.AsTask);
        
        //then
        actualEssayValidationException.Should().BeEquivalentTo(expectedEssayValidationException);

        this.loggingBrokerMock.Verify(broker =>
                        broker.LogError(It.Is(SameExceptionAs(expectedEssayValidationException))),
            Times.Once);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}