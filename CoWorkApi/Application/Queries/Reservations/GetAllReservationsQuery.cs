using MediatR;

public class GetAllReservationsQuery : IRequest<IEnumerable<ReservationAllDto>> { }