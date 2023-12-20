using EssayAnalyzer.Api.Models.Foundation.Essays;
using EssayAnalyzer.Api.Models.Foundation.Essays.Exceptions;
using EssayAnalyzer.Api.Services.Foundation.Essays;
using EssayAnalyzer.Api.Services.Foundation.Essays.Exceptions;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace EssayAnalyzer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EssaysController : RESTFulController
{
    private readonly IEssayService essayService;

    public EssaysController(IEssayService essayService) =>
        this.essayService = essayService;
    
    [HttpPost("post/essay")]
    public async ValueTask<ActionResult<Essay>> PostEssayAsync(Essay essay)
    {
        try
        {
            Essay addEssayAsync = 
                await this.essayService.AddEssayAsync(essay);

            return Created(addEssayAsync);
        }
        catch (EssayValidationException essayValidationException)
        {
            return BadRequest(essayValidationException.InnerException);
        }
        catch (EssayDependencyException essayDependencyException)
            when (essayDependencyException.InnerException is AlreadyExitsEssayException)
        {
            return Conflict(essayDependencyException.InnerException);
        }
        catch (EssayDependencyException essayDependencyException)
        {
            return InternalServerError(essayDependencyException.InnerException);
        }
        catch (EssayServiceException essayServiceException)
        {
            return InternalServerError(essayServiceException.InnerException);
        }
    }
    
    [HttpGet("get/all-essay")]
    public ActionResult<IQueryable<Essay>> GetAllEssays()
    {
        try
        {
            IQueryable<Essay> essays = this.essayService.RetrieveAllEssays();

            return Ok(essays);
        }
        catch (EssayDependencyException essayDependencyException)
        {
            return InternalServerError(essayDependencyException.InnerException);
        }
        catch (EssayServiceException essayServiceException)
        {
            return InternalServerError(essayServiceException.InnerException);
        }
      
    }

    [HttpGet("get/essay-by-id/{guid}")]
    public async ValueTask<ActionResult<Essay>> GetEssayById(Guid id)
    {
        try
        {
            return await this.essayService.RetrieveEssayByIdAsync(id);

            // return Ok(retrievedEssay);
        }
        catch (EssayDependencyException essayDependencyException)
        {
            return InternalServerError(essayDependencyException.InnerException);
        }
        catch (EssayValidationException essayValidationException)
            when (essayValidationException.InnerException is InvalidEssayException)
        {
            return BadRequest(essayValidationException.InnerException);
        }
        catch (EssayValidationException essayValidationException)
            when (essayValidationException.InnerException is NotFoundEssayException)
        {
            return NotFound(essayValidationException.InnerException);
        }
        catch (EssayServiceException essayServiceException)
        {
            return InternalServerError(essayServiceException.InnerException);
        }
    }
}
