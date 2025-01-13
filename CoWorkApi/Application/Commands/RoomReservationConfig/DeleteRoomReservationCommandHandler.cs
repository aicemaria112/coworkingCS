using CoWorkApi.Infraestructure.Data;
using MediatR;

namespace Application.Commands.RoomReservationConfig;

public class DeleteRoomReservationCommandHandler : IRequestHandler<DeleteRoomReservationCommand, int>
{
    private readonly AppDbContext _context;

    public DeleteRoomReservationCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(DeleteRoomReservationCommand request, CancellationToken cancellationToken)
    {
        var roomReservationConfig = await _context.RoomReservationsConfigs.FindAsync(request.Id);
        if (roomReservationConfig == null)
        {
            throw new NotFoundException("Room reservation config not found");
        }
        _context.RoomReservationsConfigs.Remove(roomReservationConfig);
        await _context.SaveChangesAsync(cancellationToken);
        return roomReservationConfig.Id;
    }
}