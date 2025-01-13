using MediatR;

namespace Application.Commands.RoomReservationConfig;

public class DeleteRoomReservationCommand : IRequest<int>
{
    public int Id { get; set; }
}