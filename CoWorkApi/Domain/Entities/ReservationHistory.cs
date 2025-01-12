using System;

namespace CoWorkApi.Domain.Entities
{
    public class ReservationHistory
    {
        public int Id { get; set; } // Clave primaria
        public int ReservationId { get; set; } // ID de la reserva asociada
        public string ChangeType { get; set; } = string.Empty; // Tipo de cambio ('create', 'update', 'cancel')
        public string? ChangeDescription { get; set; } // Descripción opcional del cambio
        public int? ChangedBy { get; set; } // ID del usuario que realizó el cambio
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow; // Fecha del cambio

        // Relaciones
        public Reservation Reservation { get; set; } = null!; // Reserva asociada
        public User? ChangedByUser { get; set; } // Usuario que realizó el cambio
    }
}