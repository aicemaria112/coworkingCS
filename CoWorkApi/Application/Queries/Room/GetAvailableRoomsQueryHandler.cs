using MediatR;
using CoWorkApi.Domain.Entities;
using CoWorkApi.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

public class GetAvailableRoomsQueryHandler : IRequestHandler<GetAvailableRoomsQuery, List<RoomDto>>
{
    private readonly AppDbContext _dbContext;

    public GetAvailableRoomsQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<RoomDto>> Handle(GetAvailableRoomsQuery request, CancellationToken cancellationToken)
    {
        // Comenzamos con la consulta base de salas disponibles
        var query = _dbContext.Rooms
            .Where(r => r.IsAvailable) // Solo salas disponibles
            .AsQueryable();

        // Aplicar filtros opcionales
        if (request.MinimumCapacity.HasValue)
        {
            query = query.Where(r => r.Capacity >= request.MinimumCapacity.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Location))
        {
            query = query.Where(r => r.Location.Contains(request.Location));
        }

        if (!string.IsNullOrWhiteSpace(request.NameContains))
        {
            query = query.Where(r => r.Name.Contains(request.NameContains));
        }

        // Mapear resultados a DTO
        var availableRooms = await query
            .Select(r => new RoomDto
            {
                Id = r.Id,
                Name = r.Name,
                Capacity = r.Capacity,
                Location = r.Location,
                Description = r.Description != null ? r.Description : string.Empty,
                IsAvailable = r.IsAvailable
            })
            .ToListAsync(cancellationToken);

        return availableRooms;
    }
}
