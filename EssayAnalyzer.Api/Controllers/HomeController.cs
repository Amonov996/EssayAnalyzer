using EssayAnalyzer.Api.Services.Foundation.EssayAnalyzes;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace EssayAnalyzer.Api.Controllers;

[ApiController]
[Route("api[controller]")]
public class HomeController : RESTFulController
{
   private readonly IEssayAnalysisService essayAnalysisService;

   public HomeController(IEssayAnalysisService essayAnalysisService)
   {
      this.essayAnalysisService = essayAnalysisService;
   }

   [HttpGet]
   public ActionResult<string> GetHomeMessage() => Ok("Hello World!");

   [HttpPost]
   // [Consumes("text/plain")]
   public async ValueTask<ActionResult<string>> Post([FromBody] string essay)
   {
      var result = await this.essayAnalysisService.AnalyzeEssayAsync(essay);

      return Ok(result);
   }
}