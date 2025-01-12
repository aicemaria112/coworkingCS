using System;

namespace CockWorkApi.Domain.Entities
{
    public class Reservation
    {
        public int Id { get; set; } 
        public int UserId { get; set; } 
        public int RoomId { get; set; } 
        public int ReservatedQuantity { get; set;}
        public DateTime StartTime { get; set; } 
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = "active"; // Estado ('active', 'cancelled', 'completed')
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 

        // Relaciones
        public User User { get; set; } = null!; 
        public Room Room { get; set; } = null!; 
    }
}