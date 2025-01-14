using MediatR;
using CoWorkApi.Domain.Entities;
using CoWorkApi.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Application.Queries.RoomReservationConfig;

public class GetAvailableRoomsAdminQueryHandler : IRequestHandler<GetAvailableRoomsAdminQuery, List<RoomDto>>
{
    private readonly AppDbContext _dbContext;
    private readonly IMemoryCache _cache;
    private const string CacheKeyPrefix = "AvailableRooms";

    public GetAvailableRoomsAdminQueryHandler(AppDbContext dbContext, IMemoryCache cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<List<RoomDto>> Handle(GetAvailableRoomsAdminQuery request, CancellationToken cancellationToken)
    {
       if(request.Show != string.Empty && request.Show != "all" && request.Show!="available" && request.Show!= "noAvailable"){
          throw new  BadHttpRequestException($"Parametro para show incorrecto los valores deben ser ['all','available','noAvailable'] current: {request.Show}");
       }

        var query = _dbContext.Rooms
            .AsQueryable();

        if (request.Show=="all")
        {
            query = query.Where(r => r.IsAvailable ==true || r.IsAvailable ==false );
        }
        if (request.Show=="available")
        {
            query = query.Where(r => r.IsAvailable ==true );
        }

        if (request.Show=="noAvailable")
        {
            query = query.Where(r => r.IsAvailable ==false );
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

        return availableRooms;
    }
}
