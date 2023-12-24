using EssayAnalyzer.Api.Models.Foundation.Essays;
using EssayAnalyzer.Api.Models.Foundation.Results;
using EssayAnalyzer.Api.Models.Orchestration.Analyse;
using EssayAnalyzer.Api.Services.Foundation.EssayAnalyzes;
using EssayAnalyzer.Api.Services.Orchestration.EssayAnalyser;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace EssayAnalyzer.Api.Controllers;

[ApiController]
[Route("api[controller]")]
public class HomeController : RESTFulController
{
   private readonly IEssayAnalyseFeedbackOrchestrationService analyseFeedbackOrchestrationService;

   public HomeController(IEssayAnalyseFeedbackOrchestrationService analyseFeedbackOrchestrationService)
   {
       this.analyseFeedbackOrchestrationService = analyseFeedbackOrchestrationService;
   }

   [HttpGet]
   public ActionResult<string> GetHomeMessage() => Ok("Hello World!");

   [HttpPost]
   public async Task<ActionResult<EssayAnalyse>> Post(Essay essay)
   {
       var essayAnalyse = new EssayAnalyse();
       essayAnalyse.Essay = essay;

       await analyseFeedbackOrchestrationService
           .AnalyseEssayFeedback(essayAnalyse);
       
       return Ok(essayAnalyse);
   }
}