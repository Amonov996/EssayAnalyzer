using EssayAnalyzer.Api.Models.Foundation.Users;
using EssayAnalyzer.Api.Models.Foundation.Users.Exceptions;
using EssayAnalyzer.Api.Services.Foundation.Users;
using EssayAnalyzer.Api.Services.Foundation.Users.Exceptions;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace EssayAnalyzer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : RESTFulController
{
    private readonly IUserService userService;

    public UsersController(IUserService userService) =>
        this.userService = userService;

    [HttpPost("post/user")]
    public async ValueTask<ActionResult<User>> PostUserAsync(User user)
    {
        try
        {
            User postedUser = await this.userService.AddUserAsync(user);
            return Created(postedUser);
        }
        catch (UserValidationException userValidationException)
        {
            return BadRequest(userValidationException.InnerException);
        }
        catch (UserDependencyValidationException userDependencyValidationException)
            when (userDependencyValidationException.InnerException is InvalidUserRefenerenceException)
        {
            return FailedDependency(
                userDependencyValidationException.InnerException);
        }
        catch (UserDependencyValidationException userDependencyValidationException)
            when (userDependencyValidationException.InnerException is AlreadyExistsUserException)
        {
            return Conflict(userDependencyValidationException.InnerException);
        }
        catch (UserDependencyException userDependencyException)
        {
            return InternalServerError(userDependencyException);
        }
        catch (UserServiceException userServiceException)
        {
            return InternalServerError(userServiceException);
        }
    }

    [HttpGet]
    public ActionResult<IQueryable<User>> GetAllUsers()
    {
        try
        {
            IQueryable<User> users = this.userService
                .RetrieveAllUsers();

            return Ok(users);
        }
        catch (UserDependencyException userDependencyException)
        {
            return InternalServerError(userDependencyException);
        }
        catch (UserServiceException userServiceException)
        {
            return InternalServerError(userServiceException);
        }
    }

    [HttpGet("{id}")]
    public async ValueTask<ActionResult<User>> GetUserById(Guid id)
    {
        try
        {
            User user = await this.userService
                .RetrieveUserByIdAsync(id);

            return Ok(user);
        }
        catch (UserValidationException userValidationException)
            when (userValidationException.InnerException is NotFoundUserException)
        {
            return NotFound(userValidationException.InnerException);
        }
        catch (UserValidationException userValidationException)
        {
            return BadRequest(userValidationException.InnerException);
        }
        catch (UserDependencyException userDependencyException)
        {
            return InternalServerError(userDependencyException.InnerException);
        }
        catch (UserServiceException userServiceException)
        {
            return InternalServerError(userServiceException.InnerException);
        }
    }

    [HttpPut]
    public async ValueTask<ActionResult<User>> PutUserAsync(User user)
    {
        try
        {
            User updatedUser = await this.userService
                .ModifyUserAsync(user);

            return Ok(updatedUser);
        }
        catch (UserValidationException userValidationException)
            when (userValidationException.InnerException is NotFoundUserException)
        {
            return NotFound(userValidationException.InnerException);
        }
        catch (UserValidationException userValidationException)
        {
            return BadRequest(userValidationException.InnerException);
        }
        catch (UserDependencyValidationException userDependencyValidationException)
            when(userDependencyValidationException.InnerException is InvalidUserException)
        {
            return FailedDependency(userDependencyValidationException.InnerException);
        }
        catch (UserDependencyException userDependencyException)
        {
            return InternalServerError(userDependencyException.InnerException);
        }
        catch (UserServiceException userServiceException)
        {
            return InternalServerError(userServiceException.InnerException);
        }
    }

    [HttpDelete("{id}")]
    public async ValueTask<ActionResult<User>> DeleteUserAsync(Guid id)
    {
        try
        {
            User user = await this.userService.RemoveUserByIdAsync(id);
            return Ok(user);
        }
        catch (UserValidationException userValidationException)
            when (userValidationException.InnerException is NotFoundUserException)
        {
            return NotFound(userValidationException.InnerException);
        }
        catch (UserValidationException userValidationException)
        {
            return BadRequest(userValidationException.InnerException);
        }
        catch (UserDependencyValidationException userDependencyValidationException)
        {
            return BadRequest(userDependencyValidationException.InnerException);
        }
        catch (UserDependencyException userDependencyException)
        {
            return InternalServerError(userDependencyException.InnerException);
        }
        catch (UserServiceException userServiceException)
        {
            return InternalServerError(userServiceException.InnerException);
        }
    }
    
}