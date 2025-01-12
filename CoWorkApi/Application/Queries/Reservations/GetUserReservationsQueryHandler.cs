using CoWorkApi.Infraestructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetUserReservationsQueryHandler : IRequestHandler<GetUserReservationsQuery, IEnumerable<ReservationDto>>
{
    private readonly AppDbContext _context;

    public GetUserReservationsQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ReservationDto>> Handle(GetUserReservationsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Reservations
            .Where(r => r.UserId == request.UserId)
            .Include(r => r.Room) 
            .Select(r => new ReservationDto
            {
                Id = r.Id,
                Room = new RoomDto
                {
                    Id = r.Room.Id,
                    Name = r.Room.Name,
                    Capacity = r.Room.Capacity,
                    Location = r.Room.Location,
                    Description = r.Room.Description ?? "",
                    IsAvailable = r.Room.IsAvailable
                },
                UserId = r.User.Id,
                StartTime = r.StartTime,
                EndTime = r.EndTime,
                Status = r.Status,
            })
            .ToListAsync(cancellationToken);
    }
}
