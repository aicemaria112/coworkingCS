using MediatR;

public class GetAvailableRoomsQuery : IRequest<List<RoomDto>>
{

    public int? MinimumCapacity { get; set; } // Capacidad mínima
    public string? Location { get; set; } // Ubicación específica
    public string? NameContains { get; set; } // Buscar por nombre
}