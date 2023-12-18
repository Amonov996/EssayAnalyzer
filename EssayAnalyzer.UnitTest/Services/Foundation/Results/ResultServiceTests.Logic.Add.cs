using EssayAnalyzer.Api.Models.Foundation.Results;
using FluentAssertions;
using Force.DeepCloner;
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
        public async Task ShouldAddResultAsync()
        {
            // given
            Result randomResult = CreateRandomResult();
            Result inputResult = randomResult;
            Result persistedResult = inputResult;
            Result expectedResult = persistedResult.DeepClone();

            this.storageBrokerMock.Setup(broker =>
            broker.InsertResultAsync(inputResult))
                .ReturnsAsync(persistedResult);

            // when
            Result actualResult =
                await this.resultService.AddResultAsync(inputResult);

            // then
            actualResult.Should().BeEquivalentTo(expectedResult);

            this.storageBrokerMock.Verify(broker =>
            broker.InsertResultAsync(inputResult), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
