using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoWorkApi.Domain.Entities {

public class Room
{
    public int Id { get; set; } // Autogenerado
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public int Capacity { get; set; }
    [Required]
    public string Location { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    [Required]
    public bool IsAvailable { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Fecha actual

     public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
     public ICollection<RoomReservationsConfig> RoomReservationsConfigs { get; set; } = new List<RoomReservationsConfig>();
}
}