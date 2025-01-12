using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("list")]
    [Authorize]
    public async Task<IActionResult> GetUsersList()
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
        var query = new GetUserListQuery { };

        var result = await _mediator.Send(query);

        return Ok(result); 
    }
}
