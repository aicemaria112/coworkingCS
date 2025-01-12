using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogService _logService;


    public UserController(IMediator mediator, ILogService logService    )
    {
        _mediator = mediator;
        _logService = logService;
    }

    [HttpGet("list")]
    [Authorize]
    public async Task<IActionResult> GetUsersList()
    {
        var role = User.FindFirstValue(ClaimTypes.Role);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userId, out int _userId))
        {
            _userId = 0;
        }


        if (string.IsNullOrEmpty(role))
        {
            await _logService.AddLogAsync("GET", "/api/user/list", 401, _userId);
            return Unauthorized(new Dictionary<string, string> { {  "Message" , "User ID not found in token." } });
        }

        if (role.Equals("user"))
        {
            await _logService.AddLogAsync("GET", "/api/user/list", 401, _userId);
            return Unauthorized(new Dictionary<string, string> { {  "Message" , "You have not permission to do that." } });
        }
        var query = new GetUserListQuery { };

        var result = await _mediator.Send(query);
        await _logService.AddLogAsync("GET", "/api/user/list", 200, _userId);   
        return Ok(result); 
    }
}
