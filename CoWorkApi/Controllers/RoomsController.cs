using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogService _logService;

    private readonly IWebHostEnvironment _environment;

    public RoomsController(IMediator mediator, ILogService logService, IWebHostEnvironment environment)
    {
        _mediator = mediator;
        _logService = logService;
        _environment = environment;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAdminRooms(
        [FromQuery] string? show = "all")
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
            await _logService.AddLogAsync("GET", "/api/rooms", 403, _userId);
            return StatusCode(403, new Dictionary<string, string> { { "Message", "Only admin can perform this action" } });
        }

        var query = new GetAvailableRoomsAdminQuery
        {
            Show = show
        };
        try
        {
            var result = await _mediator.Send(query);
            return Ok(result);

        }
        catch (BadHttpRequestException e)
        {

            return BadRequest(e.Message);
        }

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
    public async Task<IActionResult> CreateRoom([FromForm] CreateRoomCommand command)
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

        if (command.Image != null)
        {
            var uploadsFolder = Path.Combine(_environment.ContentRootPath, "Uploads", "Images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + command.Image.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await command.Image.CopyToAsync(fileStream);
            }
            command.ImageUrl = "/uploads/images/" + uniqueFileName;
        }
        var result = await _mediator.Send(command);
        await _logService.AddLogAsync("POST", "/api/rooms", 201, _userId);
        return StatusCode(201, new Dictionary<string, int> { { "id", result } });
    }
}
