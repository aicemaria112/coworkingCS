public class RoomDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string ImageUrl {get; set;} = string.Empty;
    public bool IsAvailable { get; set; }
}