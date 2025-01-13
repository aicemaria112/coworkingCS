using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoWorkApi.Domain.Entities {

public class RoomReservationsConfig
{
    public int Id { get; set; } // Autogenerado
    [Required]
    public int RoomId { get; set; }
    [Required]
    public string TimeRangeName { get; set; } = string.Empty; //hora, dia, semana, mes
    [Required]
    public int TimeRangeValue { get; set; }
    [Required]
    public int Price { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Fecha actual

    public Room Room { get; set; } = null!; 
}
}