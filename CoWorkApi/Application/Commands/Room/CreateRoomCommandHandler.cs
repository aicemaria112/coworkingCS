
using CoWorkApi.Domain.Entities;
using CoWorkApi.Infraestructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, int>
{
    private readonly AppDbContext _context;
 
    public CreateRoomCommandHandler(AppDbContext context)
    {
        _context = context;
   
    }

    public async Task<int> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = new Room
        {
            Name = request.Name,
            Capacity = request.Capacity,
            Description = request.Description,
            Location = request.Location,
            IsAvailable = request.IsAvailable,
            ImageUrl = request.ImageUrl
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync(cancellationToken);

        return room.Id;
    }
}
