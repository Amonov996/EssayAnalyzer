using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace EssayAnalyzer.Api.Controllers;

[ApiController]
[Route("api[controller]")]
public class HomeController : RESTFulController
{
   [HttpGet]
   public ActionResult<string> GetHomeMessage() => Ok("Hello World!");
}