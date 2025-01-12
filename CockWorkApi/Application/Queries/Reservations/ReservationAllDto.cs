using System.ComponentModel.DataAnnotations;

public class ReservationAllDto
{
    public int Id { get; set; }
    public RoomDto Room { get; set; } 
    public UserInfoDto? User { get; set; } 
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; }
}