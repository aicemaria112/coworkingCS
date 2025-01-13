using System.Security.Claims;
using Application.Commands.RoomReservationConfig;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoWorkApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomReservationConfigController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogService _logService;

    public RoomReservationConfigController(IMediator mediator, ILogService logService)
    {
        _mediator = mediator;
        _logService = logService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoomReservationConfig(RoomReservationConfigCommand command)
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
            await _logService.AddLogAsync("POST", "/api/roomreservationconfig", 403, _userId);
            return StatusCode(403, new Dictionary<string, string> { { "Message", "Only admin can perform this action" } });
        }
        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception)
        {
            return BadRequest(new Dictionary<string, string> { { "Message", "Error creating room reservation config" } });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoomReservationConfig(int id)
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
            await _logService.AddLogAsync("DELETE", "/api/roomreservationconfig", 403, _userId);
            return StatusCode(403, new Dictionary<string, string> { { "Message", "Only admin can perform this action" } });
        }
        try
        {
            var result = await _mediator.Send(new DeleteRoomReservationCommand { Id = id });
            return Ok(result);
        }
        catch (Exception)
        {
            return BadRequest(new Dictionary<string, string> { { "Message", "Error deleting room reservation config" } });
        }
    }

}