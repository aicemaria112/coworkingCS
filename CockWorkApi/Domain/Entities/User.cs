using System;
using System.Collections.Generic;

namespace CockWorkApi.Domain.Entities
{
    public class User
    {
        public int Id { get; set; } // Clave primaria
        public string Username { get; set; } = string.Empty; // Nombre de usuario único
        public string Password { get; set; } = string.Empty; // Contraseña
        public string Email { get; set; } = string.Empty; // Correo electrónico único
        public string Role { get; set; } = "user"; // Rol ('admin', 'user')
        public string Name {get; set;} = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Fecha de creación

        // Relación con las reservas realizadas por el usuario
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}