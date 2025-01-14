
using CoWorkApi.Domain.Entities;
using CoWorkApi.Infraestructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, int>
{
    private readonly AppDbContext _context;
    private readonly IReservationHistoryService _historyService;
    public CreateReservationCommandHandler(AppDbContext context, IReservationHistoryService historyService)
    {
        _context = context;
        _historyService = historyService;
    }

    public async Task<int> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        //descartado es mas lento que directo any async
        //     var conflictingReservations = await _context.Reservations
        // .Where(r => r.RoomId == request.RoomId &&
        //             r.StartTime < request.EndTime &&
        //             r.EndTime > request.StartTime)
        // .ToListAsync(cancellationToken);

        //     bool exists = conflictingReservations.Any();
        // Verificar disponibilidad

        var roomExists = await _context.Rooms.AnyAsync(r => r.Id == request.RoomId, cancellationToken);
        if (!roomExists)
        {
            throw new NotFoundException($"The room with ID {request.RoomId} does not exist.");
        }

        bool exists = await _context.Reservations
            .AnyAsync(r => r.RoomId == request.RoomId &&
                           r.StartTime < request.EndTime &&
                           r.Status != "cancelled" &&
                           r.EndTime > request.StartTime, cancellationToken);

        if (exists)
        {
            throw new ConflictException("The room is already reserved for the specified time range.");
        }

        var reservation = new Reservation
        {
            UserId = request.UserId,
            RoomId = request.RoomId,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            ReservatedQuantity = request.Quantity,
            Status = "active"
        };

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync(cancellationToken);

        await _historyService.AddHistoryAsync(
            reservation.Id,
            "active",
            "Reserva creada",
            request.UserId);    
        return reservation.Id;
    }
}
