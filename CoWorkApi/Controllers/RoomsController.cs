using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogService _logService;

    public RoomsController(IMediator mediator, ILogService logService)
    {
        _mediator = mediator;
        _logService = logService;
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableRooms(
        [FromQuery] int? minimumCapacity,
        [FromQuery] string? location,
        [FromQuery] string? nameContains)
    {
        var query = new GetAvailableRoomsQuery
        {
            MinimumCapacity = minimumCapacity,
            Location = location,
            NameContains = nameContains
        };

        var result = await _mediator.Send(query);

        return Ok(result); // Retornar la lista filtrada
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomCommand command)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userId, out int _userId))
        {
            _userId = 0;
        }
        if (_userId == 0)
        {
            return Unauthorized(new Dictionary<string, string> { { "Message", "You must be logged to do that" } });
        }
        var role = User.FindFirstValue(ClaimTypes.Role);
        if (role != "admin")
        {
            await _logService.AddLogAsync("POST", "/api/rooms", 403, _userId);
            return StatusCode(403, new Dictionary<string, string> { { "Message", "Only admin can perform this action" } });
        }
        var result = await _mediator.Send(command);
        await _logService.AddLogAsync("POST", "/api/rooms", 201, _userId);
        return StatusCode(201, new Dictionary<string, int> { { "id", result } });
    }
}
