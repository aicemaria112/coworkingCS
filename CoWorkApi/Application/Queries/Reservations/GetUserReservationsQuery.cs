using MediatR;

public class GetUserReservationsQuery : IRequest<IEnumerable<ReservationDto>>
{
    public int UserId { get; set; }
}