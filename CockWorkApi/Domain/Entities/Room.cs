using System;
using System.Collections.Generic;

namespace CockWorkApi.Domain.Entities {

public class Room
{
    public int Id { get; set; } // Autogenerado
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string Location { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsAvailable { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Fecha actual

     public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
}