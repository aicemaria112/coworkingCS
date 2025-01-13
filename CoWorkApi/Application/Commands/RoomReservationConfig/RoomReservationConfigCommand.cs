using MediatR;

namespace Application.Commands.RoomReservationConfig;

public class RoomReservationConfigCommand : IRequest<int>
{
    public int RoomId { get; set; }
    public string TimeRangeName { get; set; } = string.Empty;
    public int TimeRangeValue { get; set; }
    public string Name { get; set; } = string.Empty;

    public int Price { get; set; }


}
