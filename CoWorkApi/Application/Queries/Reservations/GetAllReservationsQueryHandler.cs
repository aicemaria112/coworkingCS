using CoWorkApi.Infraestructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetAllReservationsQueryHandler : IRequestHandler<GetAllReservationsQuery, IEnumerable<ReservationAllDto>>
{
    private readonly AppDbContext _context;

    public GetAllReservationsQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ReservationAllDto>> Handle(GetAllReservationsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Reservations
            .Include(r => r.Room) // Incluir datos de la sala
            .Include(r => r.User) // Incluir datos del usuario
            .Select(r => new ReservationAllDto
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
                User = new UserInfoDto
                {
                    Username = r.User.Username,
                    Email = r.User.Email,
                    Name = r.User.Name,
                    Role= r.User.Role,
                    Id=r.Id
                },
                StartTime = r.StartTime,
                EndTime = r.EndTime,
                Status = r.Status
            })
            .ToListAsync(cancellationToken);
    }
}
