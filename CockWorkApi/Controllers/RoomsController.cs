using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoomsController(IMediator mediator)
    {
        _mediator = mediator;
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
}
