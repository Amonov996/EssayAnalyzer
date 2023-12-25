using EssayAnalyzer.Api.Models.Foundation.Results;
using EssayAnalyzer.Api.Models.Foundation.Results.Exceptions;
using EssayAnalyzer.Api.Services.Foundation.Results;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace EssayAnalyzer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResultController : RESTFulController
{
    private readonly IResultService resultService;

    public ResultController(IResultService resultService) =>
        this.resultService = resultService;

    [HttpPost]
    public async ValueTask<ActionResult<Result>> PostResultAsync(Result result)
    {
        try
        {
            Result postedResult = await this.resultService
                .AddResultAsync(result);

            return Created(postedResult);
        }
        catch (ResultValidationException resultValidationException)
        {
            return BadRequest(resultValidationException.InnerException);
        }
        catch (ResultDependencyException resultDependencyException)
        {
            return InternalServerError(resultDependencyException);
        }
        catch (ResultServiceException resultServiceException)
        {
            return InternalServerError(resultServiceException);
        }
    }

    [HttpGet]
    public ActionResult<IQueryable<Result>> GetAllResults()
    {
        try
        {
            IQueryable<Result> results = this.resultService
                .RetrieveAllResults();

            return Ok(results);
        }
        catch (ResultDependencyException resultDependencyException)
        {
            return InternalServerError(resultDependencyException);
        }
        catch (ResultServiceException resultServiceException)
        {
            return InternalServerError(resultServiceException);
        }
    }

    [HttpGet("{id}")]
    public async ValueTask<ActionResult<Result>> GetResultByIdAsync(Guid id)
    {
        try
        {
            Result result = await this.resultService
                .RetrieveResultByIdAsync(id);

            return Ok(result);
        }
        catch (ResultValidationException resultValidationException)
            when(resultValidationException.InnerException is NotFoundResultException)
        {
            return NotFound(resultValidationException.InnerException);
        }
        catch (ResultValidationException resultValidationException)
        {
            return BadRequest(resultValidationException.InnerException);
        }
        catch (ResultDependencyException resultDependencyException)
        {
            return InternalServerError(resultDependencyException);
        }
        catch (ResultServiceException resultServiceException)
        {
            return InternalServerError(resultServiceException);
        }
    }
}