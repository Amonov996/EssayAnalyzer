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

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ShouldThrowValidationExceptionOnAddIfInputIsInvalidAndLogItAsync(string invalidText)
    {
        //given
        var invalidEssay = new Essay()
        {
                    Title = invalidText,
                    Content = invalidText
        };

        var expectedInvalidEssayException = new InvalidEssayException();
        
        expectedInvalidEssayException.AddData(
            key:nameof(Essay.Id),
            values: "Id is required");
        
        expectedInvalidEssayException.AddData(
            key:nameof(Essay.UserId),
            values: "Id is required");
        
        expectedInvalidEssayException.AddData(
            key:nameof(Essay.Title),
            values: "Text is required");
        
        expectedInvalidEssayException.AddData(
            key:nameof(Essay.Content),
            values: "Text is required");

        var expectedEssayValidationException =
                    new EssayValidationException(expectedInvalidEssayException);
        
        //when
        ValueTask<Essay> addEssayTask = this.essayService.AddEssayAsync(invalidEssay);

        EssayValidationException actualEssayValidationException =
                    await Assert.ThrowsAsync<EssayValidationException>(addEssayTask.AsTask);
        
        //then
        actualEssayValidationException.Should().BeEquivalentTo(expectedEssayValidationException);
        
        this.loggingBrokerMock.Verify(broker =>
                    broker.LogError(It.Is(SameExceptionAs(expectedEssayValidationException))),
            Times.Once);

        this.storageBrokerMock.Verify(broker =>
                    broker.InsertEssayAsync(invalidEssay), Times.Never);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}