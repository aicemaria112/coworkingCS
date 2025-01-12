using CockWorkApi.Domain.Entities;
using CockWorkApi.Infraestructure.Data;
using MediatR;

public class CancelReservationCommandHandler : IRequestHandler<CancelReservationCommand>
{
    private readonly AppDbContext _context;

    public CancelReservationCommandHandler(AppDbContext context)
    {
        _context = context;
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

        reservation.Status = "cancelled";
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
