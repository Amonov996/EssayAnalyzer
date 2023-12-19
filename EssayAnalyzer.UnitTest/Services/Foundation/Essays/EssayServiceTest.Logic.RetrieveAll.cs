using System.Runtime.InteropServices.ComTypes;
using EssayAnalyzer.Api.Models.Foundation.Essays;
using FluentAssertions;
using Moq;
using Xunit;

namespace EssayAnalyzer.UnitTest.Services.Foundation.Essays;

public partial class EssayServiceTest
{
   [Fact]
   public void ShouldRetrieveEssays()
   {
      //given
      IQueryable<Essay> randomEssays = CreateRandomEssays();
      IQueryable<Essay> storageEssays = randomEssays;
      IQueryable<Essay> expectedEssays = storageEssays;

      this.storageBrokerMock.Setup(broker => 
         broker.SelectAllEssays()).Returns(storageEssays);
      
      //when
      IQueryable<Essay> actualEssays = this.essayService.RetrieveAllEssays();
      
      //then
      actualEssays.Should().BeEquivalentTo(expectedEssays);
      
      this.storageBrokerMock.Verify(broker => 
         broker.SelectAllEssays(),Times.Once);
      
      this.storageBrokerMock.VerifyNoOtherCalls();
      this.loggingBrokerMock.VerifyNoOtherCalls();
   }
}