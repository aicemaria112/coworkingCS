using MediatR;
using CoWorkApi.Domain.Entities;
using CoWorkApi.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Application.Queries.RoomReservationConfig;

public class GetAvailableRoomsQueryHandler : IRequestHandler<GetAvailableRoomsQuery, List<RoomDto>>
{
    private readonly AppDbContext _dbContext;
    private readonly IMemoryCache _cache;
    private const string CacheKeyPrefix = "AvailableRooms";

    public GetAvailableRoomsQueryHandler(AppDbContext dbContext, IMemoryCache cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<List<RoomDto>> Handle(GetAvailableRoomsQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"{CacheKeyPrefix}_{request.MinimumCapacity}_{request.Location}_{request.NameContains}";

        if (_cache.TryGetValue(cacheKey, out List<RoomDto>? cachedRooms))
        {
            return cachedRooms;
        }

        var query = _dbContext.Rooms
            .Where(r => r.IsAvailable)
            .AsQueryable();

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

        var availableRooms = await query
            .Select(r => new RoomDto
            {
                Id = r.Id,
                Name = r.Name,
                Capacity = r.Capacity,
                Location = r.Location,
                Description = r.Description ?? string.Empty,
                IsAvailable = r.IsAvailable,
                ImageUrl = r.ImageUrl ?? "",
                RoomReservationsConfigs = r.RoomReservationsConfigs.Select(rrc => new RoomReservationsConfigDto
                {
                    Id = rrc.Id,
                    TimeRangeName = rrc.TimeRangeName,
                    TimeRangeValue = rrc.TimeRangeValue,
                    Price = rrc.Price,
                    Name = rrc.Name
                }).ToList()
            })
            .ToListAsync(cancellationToken);

        _cache.Set(cacheKey, availableRooms, TimeSpan.FromMinutes(5));

        return availableRooms;
    }
}
