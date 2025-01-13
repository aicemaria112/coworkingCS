using System;

namespace Application.Queries.RoomReservationConfig;

public class RoomReservationsConfigDto
{
    public int Id { get; set; }
    public string TimeRangeName { get; set; } = string.Empty;
    public int TimeRangeValue { get; set; }
    public int Price { get; set; }
    public string Name { get; set; } = string.Empty;
}