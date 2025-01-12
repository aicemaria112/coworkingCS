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
    private readonly ILogService _logService;

    public RoleController(IMediator mediator, ILogService logService)
    {
        _mediator = mediator;
        _logService = logService;
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UserRole([FromBody] UserRoleCommand command)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userId, out int _userId))
        {
            _userId = 0;
        }
        var role = User.FindFirstValue(ClaimTypes.Role);

        if (string.IsNullOrEmpty(role))
        {
            await _logService.AddLogAsync("PUT", "/api/role", 401, _userId);
            return Unauthorized(new Dictionary<string, string> { {  "Message" , "User ID not found in token." } });
        }

        if (role.Equals("user"))
        {
            return Unauthorized(new Dictionary<string, string> { {  "Message" , "You have not permission to do that." } });
        }

         try
        {
            var message = await _mediator.Send(command);
            await _logService.AddLogAsync("PUT", "/api/role", 200, _userId);
            return Ok(new Dictionary<string, string> { {  "Message" , message } });
        }catch(NotFoundException e){
            await _logService.AddLogAsync("PUT", "/api/role", 404, _userId);
            return NotFound(new Dictionary<string, string> { {  "Message" , e.Message } });
        }catch (ForbidenAccessException e){
            await _logService.AddLogAsync("PUT", "/api/role", 403, _userId);
           return BadRequest(new Dictionary<string, string> { {  "Message" , e.Message } });
        }catch (ValidationException e){
            await _logService.AddLogAsync("PUT", "/api/role", 400, _userId);
           return BadRequest(new Dictionary<string, string> { {  "Message" , e.Message } });
        }
    }

}