
using Application.Commands.RoomReservationConfig;
using CoWorkApi.Domain.Entities;
using CoWorkApi.Infraestructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class RoomReservationConfigCommandHandler : IRequestHandler<RoomReservationConfigCommand, int>
{
    private readonly AppDbContext _context;
 
    public RoomReservationConfigCommandHandler(AppDbContext context)
    {
        _context = context;
   
    }

    public async Task<int> Handle(RoomReservationConfigCommand request, CancellationToken cancellationToken)
    {
        var roomConfig = new RoomReservationsConfig
        {
            Name = request.Name,
            TimeRangeName = request.TimeRangeName,
            TimeRangeValue = request.TimeRangeValue,
            RoomId = request.RoomId,
            Price = request.Price
        };

        _context.RoomReservationsConfigs.Add(roomConfig);
        await _context.SaveChangesAsync(cancellationToken);

        return roomConfig.Id;
    }
}
