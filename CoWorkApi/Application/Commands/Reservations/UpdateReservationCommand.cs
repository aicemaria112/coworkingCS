using MediatR;

public class UpdateReservationCommand : IRequest
{
    public int ReservationId { get; set; }
    public int UserId { get; set; }
    public int RoomId { get; set; }
    public string Status { get; set; } = "active";
    public int Quantity {get; set;}
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}