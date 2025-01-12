using System.ComponentModel.DataAnnotations;

public class ReservationDto
{
    public int Id { get; set; }
    public RoomDto Room { get; set; } 
    public int UserId { get; set; } 
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; }
}