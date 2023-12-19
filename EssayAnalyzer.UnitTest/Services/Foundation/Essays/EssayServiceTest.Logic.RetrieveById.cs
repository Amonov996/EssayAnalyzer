using EssayAnalyzer.Api.Models.Foundation.Essays;
using FluentAssertions;
using Force.DeepCloner;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Essays;

public partial class EssayServiceTest
{
    [Fact]
    public async Task ShouldRetrieveEssayByIdAsync()
    {
        //given
        Guid randomEssayId = Guid.NewGuid();
        Guid inputEssayId = randomEssayId;
        Essay randomEssay = CreateRandomEssay();
        Essay expectedEssay = randomEssay.DeepClone();

        this.storageBrokerMock.Setup(broker =>
            broker.SelectEssayByIdAsync(randomEssayId)).ReturnsAsync(randomEssay);
        
        //when
        Essay actualEssay = await this.essayService.RetrieveEssayByIdAsync(inputEssayId);
        
        //then
        actualEssay.Should().BeEquivalentTo(expectedEssay);
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectEssayByIdAsync(inputEssayId), Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}