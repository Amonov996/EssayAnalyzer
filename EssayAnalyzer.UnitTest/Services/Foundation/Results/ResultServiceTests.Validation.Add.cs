using EssayAnalyzer.Api.Models.Foundation.Results;
using EssayAnalyzer.Api.Models.Foundation.Results.Exceptions;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Results
{
    public partial class ResultServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfResultIsNullAndLogItAsync()
        {
            // given
            Result nullResult = null;
            var nullResultException = new NullResultException();

            var expectedResultValidationException =
                new ResultValidationException(nullResultException);

            // when
            ValueTask<Result> addResultTask =
                this.resultService.AddResultAsync(nullResult);

            ResultValidationException actualResultValidationException =
                await Assert.ThrowsAsync<ResultValidationException>(addResultTask.AsTask);

            // then
            actualResultValidationException.Should().BeEquivalentTo(
                expectedResultValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                   expectedResultValidationException))), Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Xunit.Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("   ")]

        public async Task ShouldThrowValidationExceptionOnAddIfResultIsInvalidAndLogItAsync(
                    string invalidText)
        {
            // given
            var invalidResult = new Result
            {
                Feedback = invalidText
            };

            var invalidResultException = new InvalidResultException();

            invalidResultException.AddData(
                key: nameof(Result.Id),
                values: "Id is required.");

            invalidResultException.AddData(
                key: nameof(Result.Feedback),
                values: "Text is required.");

            var expectedResultValidationException =
                new ResultValidationException(invalidResultException);

            // when
            ValueTask<Result> addResultTask =
                this.resultService.AddResultAsync(invalidResult);

            ResultValidationException actualResultValidationException =
                await Assert.ThrowsAsync<ResultValidationException>(addResultTask.AsTask);

            // then 
            actualResultValidationException.Should().BeEquivalentTo(
                expectedResultValidationException);

            this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(
                expectedResultValidationException))), Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
