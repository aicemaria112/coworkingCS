using MediatR;

public class CreateRoomCommand : IRequest<int>
{
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public IFormFile? Image { get; set; }
    public string ImageUrl { get; set; } = string.Empty;

    public bool IsAvailable { get; set; } = true;
}
