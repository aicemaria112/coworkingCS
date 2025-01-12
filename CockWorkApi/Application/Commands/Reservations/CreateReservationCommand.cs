using MediatR;

public class CreateReservationCommand : IRequest<int>
{
    public int UserId { get; set; }
    public int RoomId { get; set; }
    public int Quantity {get; set;}
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
