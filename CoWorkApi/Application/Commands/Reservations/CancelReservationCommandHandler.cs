
using CoWorkApi.Domain.Entities;
using CoWorkApi.Infraestructure.Data;
using MediatR;

public class CancelReservationCommandHandler : IRequestHandler<CancelReservationCommand>
{
    private readonly AppDbContext _context;
    private readonly IReservationHistoryService _historyService;

    public CancelReservationCommandHandler(AppDbContext context, IReservationHistoryService historyService)
    {
        _context = context;
        _historyService = historyService;
    }

    public async Task<Unit> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _context.Reservations.FindAsync(request.ReservationId);

        if (reservation == null)
        {
            throw new NotFoundException("Reservation", request.ReservationId);
        }

        // Verificar permiso
        if (reservation.UserId != request.UserId )
        {
            User user = _context.Users.Where(r=>r.Id == request.UserId).First();
            if(!user.Role.Equals("admin"))
            throw new UnauthorizedException("you have no rigth to do that");
        }
        var prevStatus = reservation.Status;
        reservation.Status = "cancelled";
        await _historyService.AddHistoryAsync(
            reservation.Id,
            "cancel",
            $"Reserva cancelada prevStatus: {prevStatus} to {reservation.Status}",
            request.UserId);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
