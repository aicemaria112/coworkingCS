using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]

public class RoleController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UserRole([FromBody] UserRoleCommand command)
    {

        var role = User.FindFirstValue(ClaimTypes.Role);

        if (string.IsNullOrEmpty(role))
        {
            return Unauthorized(new Dictionary<string, string> { {  "Message" , "User ID not found in token." } });
        }

        if (role.Equals("user"))
        {
            return Unauthorized(new Dictionary<string, string> { {  "Message" , "You have not permission to do that." } });
        }

         try
        {
            var message = await _mediator.Send(command);
            return Ok(new Dictionary<string, string> { {  "Message" , message } });
        }catch(NotFoundException e){
            return NotFound(new Dictionary<string, string> { {  "Message" , e.Message } });
        }catch (ForbidenAccessException e){
           return BadRequest(new Dictionary<string, string> { {  "Message" , e.Message } });
        }catch (ValidationException e){
           return BadRequest(new Dictionary<string, string> { {  "Message" , e.Message } });
        }
    }

}