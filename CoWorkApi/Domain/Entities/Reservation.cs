using System;
using System.ComponentModel.DataAnnotations;

namespace CoWorkApi.Domain.Entities
{
    public class Reservation
    {
        public int Id { get; set; } 
        [Required]
        public int UserId { get; set; } 
        [Required]
        public int RoomId { get; set; } 
        [Required]
        public int ReservatedQuantity { get; set;}
        [Required]
        public DateTime StartTime { get; set; } 
        [Required]
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = "active"; // Estado ('active', 'cancelled', 'completed')
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 

        // Relaciones
        public User User { get; set; } = null!; 
        public Room Room { get; set; } = null!; 
    }
}