using MediatR;

public class CancelReservationCommand : IRequest
{
    public int ReservationId { get; set; }
    public int UserId { get; set; }
}