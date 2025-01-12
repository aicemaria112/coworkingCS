using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        try
        {
             var result = await _mediator.Send(command);
            //return Ok(new {Message =result});
            return Ok(new Dictionary<string, string> { { "Message", result } });
            
        }
        catch (InvalidOperationException e)
        {
           return BadRequest(new Dictionary<string, string> { { "Message", e.Message } });
        }
       
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
         try
        {
            var token = await _mediator.Send(command);
            return Ok(new Dictionary<string, string> { { "Token",token } });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new Dictionary<string, string> { {  "Message" , "Credenciales inválidas." } });    
        }
    }

    [HttpGet("user-info")]
        [Authorize] // Only authenticated users can access
        public async Task<IActionResult> GetUserInfo()
        {
            // Get the user ID from the JWT token (assuming the token has a claim for 'sub' or 'nameidentifier')
           
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new Dictionary<string, string> { {  "Message" , "User ID not found in token." } });
            }

            // Create a query to fetch user info
            var query = new GetUserInfoQuery
            {
                UserId = userId
            };

            // Send the query using MediatR and get the result
            var userInfo = await _mediator.Send(query);

            if (userInfo == null)
            {
                return NotFound(new Dictionary<string, string> { {  "Message" , "User not found." } });
            }
            return Ok(userInfo);
        }
}
