using EssayAnalyzer.Api.Models.Foundation.Essays;
using EssayAnalyzer.Api.Models.Foundation.Essays.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Essays;

public partial class EssayServiceTest
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
    {
        // given
        Guid invalidId = Guid.Empty;

        var invalidEssayException =
            new InvalidEssayException();

        invalidEssayException.AddData(
            key: nameof(Essay.Id),
            values: "Id is required");

        var expectedEssayValidationException =
            new EssayValidationException(invalidEssayException);

        // when
        ValueTask<Essay> removeEssayTask = this.essayService.RemoveEssayByIdAsync(invalidId);

        EssayValidationException actualEssayValidationException =
            await Assert.ThrowsAsync<EssayValidationException>(removeEssayTask.AsTask);

        // then
        actualEssayValidationException.Should().BeEquivalentTo(expectedEssayValidationException);

        this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedEssayValidationException))),
            Times.Once);

        this.storageBrokerMock.Verify(broker =>
            broker.SelectEssayByIdAsync(It.IsAny<Guid>()), Times.Never);

        this.storageBrokerMock.Verify(broker =>
            broker.DeleteEssayAsync(It.IsAny<Essay>()), Times.Never);

        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionOnRemoveIfEssayIsNotFoundAndLogItAsync()
    {
        // given
        Guid inputEssayId = Guid.NewGuid();
        Essay noEssay = null;

        var notFoundEssayException =
            new NotFoundEssayException(inputEssayId);

        var expectedEssayValidationException =
            new EssayValidationException(notFoundEssayException);

        // when
        ValueTask<Essay> removeEssayTask = this.essayService.RemoveEssayByIdAsync(inputEssayId);

        EssayValidationException actualEssayValidationException =
            await Assert.ThrowsAsync<EssayValidationException>(removeEssayTask.AsTask);

        // then
        actualEssayValidationException.Should().BeEquivalentTo(expectedEssayValidationException);

        this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedEssayValidationException))),
            Times.Once);

        this.storageBrokerMock.Verify(broker =>
            broker.SelectEssayByIdAsync(It.IsAny<Guid>()), Times.Once);

        this.storageBrokerMock.Verify(broker =>
            broker.DeleteEssayAsync(It.IsAny<Essay>()), Times.Never);

        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}