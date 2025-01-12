
using CoWorkApi.Domain.Entities;
using CoWorkApi.Infraestructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class UpdateReservationCommandHandler : IRequestHandler<UpdateReservationCommand>
{
    private readonly AppDbContext _context;
    private readonly IReservationHistoryService _historyService;

    public UpdateReservationCommandHandler(AppDbContext context, IReservationHistoryService historyService)
    {
        _context = context;
        _historyService = historyService;
    }

    public async Task<Unit> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
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
            throw new UnauthorizedException();
        }

        var roomExists = await _context.Rooms.AnyAsync(r => r.Id == request.RoomId, cancellationToken);
        if (!roomExists)
        {
            throw new NotFoundException($"The room with ID {request.RoomId} does not exist.");
        }


        // Verificar disponibilidad
        bool exists = await _context.Reservations
            .AnyAsync(r => r.RoomId == request.RoomId &&
                           r.Id != request.ReservationId &&
                           r.StartTime < request.EndTime &&
                           r.EndTime > request.StartTime, cancellationToken);

        if (exists)
        {
            throw new ConflictException("The room is already reserved for the specified time range.");
        }

        var prevStatus = reservation.Status;

        reservation.RoomId = request.RoomId;
        reservation.StartTime = request.StartTime;
        reservation.EndTime = request.EndTime;
        reservation.ReservatedQuantity = request.Quantity;
        reservation.Status = request.Status;

        await _historyService.AddHistoryAsync(
            reservation.Id,
             request.Status,
            $"Reserva actualizada prevStatus: {prevStatus} to {request.Status}",
            request.UserId);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
