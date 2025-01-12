using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReservationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserReservations()
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
        var query = new GetUserReservationsQuery { UserId = _userId };
        var reservations = await _mediator.Send(query);
        return Ok(reservations);
    }

    [HttpGet("all")]
    [Authorize]
    public async Task<IActionResult> GetAllReservations()
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
            return StatusCode(403, new Dictionary<string, string> { { "Message", "Only admin can perform this action" } });
        }
        var reservations = await _mediator.Send(new GetAllReservationsQuery());
        return Ok(reservations);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateReservation(CreateReservationCommand command)
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
        try
        {
            command.UserId = _userId;
            var reservationId = await _mediator.Send(command);
            return StatusCode(201, new Dictionary<string, int> { { "reservation_id", reservationId } });
        }
        catch (NotFoundException e)
        {
            return NotFound(new Dictionary<string, string> { { "Message", e.Message } });
        }
        catch (ConflictException e)
        {
            return StatusCode(409, new Dictionary<string, string> { { "Message", e.Message } });
        }

    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateReservation(int id, UpdateReservationCommand command)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userId, out int _userId))
        {
            _userId = 0;
        }
        if (_userId == 0)
        {
            return Unauthorized(new Dictionary<string, string> { {  "Message" , "You must be logged to do that" } });
        }

        command.ReservationId = id;
        command.UserId = _userId;
        try
        {
            await _mediator.Send(command);
            return Ok(new Dictionary<string, string> { {  "Message" , "Success" } });
        }
        catch (NotFoundException e)
        {
            return NotFound(new Dictionary<string, string> { {  "Message" , e.Message } });
        }
        catch (ConflictException e)
        {
            return StatusCode(409, new Dictionary<string, string> { {  "Message" , e.Message } });
        }
        catch (UnauthorizedException e)
        {
            return StatusCode(403, new Dictionary<string, string> { {  "Message" , e.Message } });
        }

    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> CancelReservation(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userId, out int _userId))
        {
            _userId = 0;
        }
        if (_userId == 0)
        {
            return Unauthorized(new Dictionary<string, string> { {  "Message" , "You must be logged to do that" } });
        }
        var command = new CancelReservationCommand
        {
            ReservationId = id,
            UserId = _userId
        };

        try
        {
            await _mediator.Send(command);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(new Dictionary<string, string> { {  "Message" , e.Message } });

        }
        catch (UnauthorizedException e)
        {
            return StatusCode(403, new Dictionary<string, string> { {  "Message" , e.Message } });
        }


    }
}
