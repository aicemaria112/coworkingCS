using MediatR;

public class GetAvailableRoomsAdminQuery : IRequest<List<RoomDto>>
{

    public string? Show { get; set; } = "all";
}